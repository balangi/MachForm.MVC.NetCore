using System.Net;
using MachForm.NetCore.Entities;
using MachForm.NetCore.Models.Account;
using MachForm.NetCore.Models.Folders;
using MachForm.NetCore.Services.MainSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MachForm.NetCore.Services.Auth;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UserService> _logger;
    private readonly IMainSettingsService _mainSettingsService;

    public UserService(ApplicationDbContext dbContext, ILogger<UserService> logger, IMainSettingsService mainSettingsService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mainSettingsService = mainSettingsService;
    }

    public int AllCount()
    {
        return _dbContext.Users.Count();
    }

    public int EnableCount()
    {
        return _dbContext
            .Users
            .Where(w => w.IsActive == true)
            .Count();
    }

    public List<UserDto> GetAll()
    {
        return _dbContext.Users.ToList();
    }

    public UserDto GetItem(int id)
    {
        return _dbContext
            .Users
            .Where(w => w.Id == id.ToString())
            .FirstOrDefault();
    }

    public async Task<bool> DatabaseTablesExist()
    {
        try
        {
            // بررسی آیا migrations اعمال شده‌اند یا خیر
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
            return !pendingMigrations.Any();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking migrations status");
            return false;
        }
    }

    public async Task<bool> IsIpWhitelisted(string ipAddress)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;

            // تبدیل IP به فرمت استاندارد برای مقایسه
            var normalizedIp = NormalizeIpAddress(ipAddress);

            // بررسی وجود IP در لیست سفید
            var isWhitelisted = await _dbContext.IpWhitelists
                .AsNoTracking()
                .AnyAsync(ip => MatchIpAddress(ip.IpAddress, normalizedIp));

            return isWhitelisted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking IP whitelist for {IpAddress}", ipAddress);
            return false;
        }
    }

    private string NormalizeIpAddress(string ipAddress)
    {
        try
        {
            // تبدیل IP به فرمت استاندارد
            return IPAddress.Parse(ipAddress).ToString();
        }
        catch
        {
            return ipAddress.Trim();
        }
    }

    private bool MatchIpAddress(string whitelistIp, string requestIp)
    {
        // اگر IP دقیقا مطابقت دارد
        if (whitelistIp == requestIp)
            return true;

        // اگر IP دارای wildcard است (مثلاً 192.168.1.*)
        if (whitelistIp.Contains('*'))
        {
            var whitelistParts = whitelistIp.Split('.');
            var requestParts = requestIp.Split('.');

            if (whitelistParts.Length != 4 || requestParts.Length != 4)
                return false;

            for (int i = 0; i < 4; i++)
            {
                if (whitelistParts[i] != "*" && whitelistParts[i] != requestParts[i])
                    return false;
            }

            return true;
        }

        // بررسی range IP (مثلاً 192.168.1.1-192.168.1.100)
        if (whitelistIp.Contains('-'))
        {
            var rangeParts = whitelistIp.Split('-');
            if (rangeParts.Length != 2)
                return false;

            var lowerIp = NormalizeIpAddress(rangeParts[0]);
            var upperIp = NormalizeIpAddress(rangeParts[1]);

            return IsIpInRange(requestIp, lowerIp, upperIp);
        }

        return false;
    }

    private bool IsIpInRange(string ipAddress, string lowerIp, string upperIp)
    {
        try
        {
            var ip = IPAddress.Parse(ipAddress).GetAddressBytes();
            var lower = IPAddress.Parse(lowerIp).GetAddressBytes();
            var upper = IPAddress.Parse(upperIp).GetAddressBytes();

            // مقایسه هر بخش از IP
            for (int i = 0; i < 4; i++)
            {
                if (ip[i] < lower[i] || ip[i] > upper[i])
                    return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task RunCronJobs(bool forceRun = false)
    {
        try
        {
            // 1. پاک کردن توکن‌های منقضی شده
            await CleanupExpiredTokens();

            // 2. غیرفعال کردن کاربران غیرفعال طولانی مدت
            await DeactivateInactiveUsers();

            // 3. پاک کردن لاگ‌های قدیمی
            await CleanupOldLogs();

            // 4. اجرای نگهداری داده‌ها (Data Retention)
            await ApplyDataRetentionPolicy();

            // 5. به‌روزرسانی آخرین زمان اجرای Cron Jobs
            if (!forceRun)
            {
                await UpdateLastCronRunTime();
            }

            _logger.LogInformation("Cron jobs executed successfully at {Time}", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing cron jobs");
            throw;
        }
    }

    private async Task CleanupExpiredTokens()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-7);
        var expiredTokens = await _dbContext.UserTokens
            .Where(t => t.ExpiryDate < DateTime.UtcNow ||
                       (t.CreatedDate < cutoffDate && t.TokenType == TokenType.PasswordReset))
            .ToListAsync();

        if (expiredTokens.Any())
        {
            _dbContext.UserTokens.RemoveRange(expiredTokens);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Cleaned up {expiredTokens.Count} expired tokens");
        }
    }

    private async Task DeactivateInactiveUsers()
    {
        var cutoffDate = DateTime.UtcNow.AddMonths(-6);
        var inactiveUsers = await _dbContext.Users
            .Where(u => u.LastLoginDate < cutoffDate &&
                       u.IsActive &&
                       u.AccountType == AccountType.Regular)
            .ToListAsync();

        foreach (var user in inactiveUsers)
        {
            user.IsActive = false;
            user.DeactivationReason = "Automatic deactivation due to inactivity";
            user.DeactivatedDate = DateTime.UtcNow;
        }

        if (inactiveUsers.Any())
        {
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Deactivated {inactiveUsers.Count} inactive users");
        }
    }

    private async Task CleanupOldLogs()
    {
        var retentionDays = 90; // نگهداری لاگ‌ها به مدت 90 روز
        var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);

        var oldLogs = await _dbContext.Logs
            .Where(l => l.Timestamp < cutoffDate)
            .ToListAsync();

        if (oldLogs.Any())
        {
            _dbContext.Logs.RemoveRange(oldLogs);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Cleaned up {Count} old log entries", oldLogs.Count);
        }
    }

    private async Task ApplyDataRetentionPolicy()
    {
        var settings = await _dbContext.MainSettings
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (settings?.EnableDataRetention != true || settings.DataRetentionPeriod <= 0)
            return;

        var cutoffDate = DateTime.UtcNow.AddMonths(-settings.DataRetentionPeriod);

        // پاک کردن فرم‌های قدیمی که تگ skipdataretention ندارند
        var formsToClean = await _dbContext.Forms
            .Where(f => f.CreatedDate < cutoffDate &&
                       !f.Tags.Contains("skipdataretention"))
            .ToListAsync();

        foreach (var form in formsToClean)
        {
            // پاک کردن تمام داده‌های مرتبط با فرم
            await _dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM FormEntries WHERE FormId = {0}",
                form.Id);

            await _dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM FormElements WHERE FormId = {0}",
                form.Id);

            _dbContext.Forms.Remove(form);
        }

        if (formsToClean.Any())
        {
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Applied data retention policy to {formsToClean.Count} forms");
        }
    }

    private async Task UpdateLastCronRunTime()
    {
        var settings = await _dbContext.MainSettings.FirstOrDefaultAsync();
        if (settings != null)
        {
            settings.LastCronRun = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task CreateDefaultFolder(string userId)
    {
        try
        {
            // بررسی وجود کاربر
            var userExists = await _dbContext.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                throw new ArgumentException("User not found", nameof(userId));
            }

            // بررسی وجود پوشه پیش‌فرض
            var hasDefaultFolder = await _dbContext.Folders
                .AnyAsync(f => f.UserId ==  Convert.ToInt32( userId) && f.FolderId == 1);

            if (hasDefaultFolder)
            {
                _logger.LogInformation("User {UserId} already has default folder", userId);
                return;
            }

            // ایجاد پوشه پیش‌فرض
            var defaultFolder = new FolderDto
            {
                UserId = Convert.ToInt32(userId),
                FolderId = 1, // ID پیش‌فرض برای پوشه اصلی
                Position = 1,
                Name = "All Forms",
                Selected = true,
                RuleAllAny = "all"
            };

            _dbContext.Folders.Add(defaultFolder);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Default folder created for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating default folder for user {UserId}", userId);
            throw; // Re-throw برای مدیریت در لایه بالاتر
        }
    }

    public async Task UpdateLastLoginInfo(string userId, string ipAddress)
    {
        try
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            // به‌روزرسانی اطلاعات آخرین ورود
            user.LastLoginDate = DateTime.UtcNow;
            user.LastIpAddress = ipAddress;

            // ریست کردن شمارنده تلاش‌های ناموفق
            user.LoginAttemptCount = 0;
            user.LoginAttemptDate = null;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // در اینجا می‌توانید خطا را لاگ کنید
            throw new Exception("Failed to update last login info", ex);
        }
    }

    public async Task<bool> TwoStepVerificationRequired(string userId)
    {
        // بررسی تنظیمات سیستم برای اعمال اجباری 2-Step Verification
        var systemSettings = await _mainSettingsService.GetMainSettingsAsync();
        if (systemSettings.Security.EnforceTwoStepVerification)
        {
            return true;
        }

        // بررسی تنظیمات خاص کاربر
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }

        // اگر کاربر تنظیم کرده باشد یا اجباری باشد
        return user.TwoFactorEnabled || user.IsTwoFactorEnforced;
    }

    public async Task<LockoutRecordDto> GetByUserId(string userId)
    {
        return await _dbContext.LockoutRecords
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.LockoutStart)
            .FirstOrDefaultAsync();
    }
}