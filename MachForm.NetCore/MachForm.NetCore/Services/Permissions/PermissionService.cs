using Microsoft.EntityFrameworkCore;

namespace MachForm.NetCore.Services.Permissions
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _dbContext;

        public PermissionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Dictionary<string, bool>> GetUserPrivileges(string userId)
        {
            var privileges = new Dictionary<string, bool>
            {
                ["IsAdminister"] = false,
                ["NewForms"] = false,
                ["NewThemes"] = false,
                ["EditForm"] = false,
                ["EditEntries"] = false,
                ["ViewEntries"] = false,
                ["EditReport"] = false
            };

            // بررسی مجوزهای عمومی کاربر
            var userPermissions = await _dbContext.Permissions
                .Where(up => up.UserId == userId.ToString())
                .ToListAsync();

            // اگر کاربر دسترسی ادمین دارد، تمام مجوزها را true می‌کنیم
            if (userPermissions.Any(up => up.Administer))
            {
                foreach (var key in privileges.Keys.ToList())
                {
                    privileges[key] = true;
                }
                return privileges;
            }

            // بررسی مجوزهای اختصاصی
            foreach (var permission in userPermissions)
            {
                privileges["EditForm"] |= permission.EditForm;
                privileges["EditEntries"] |= permission.EditEntries;
                privileges["ViewEntries"] |= permission.ViewEntries;
                privileges["EditReport"] |= permission.EditReport;
            }

            // بررسی مجوزهای عمومی (غیر وابسته به فرم خاص)
            var globalPermissions = await _dbContext.GlobalPermissions
                .Where(ugp => ugp.UserId == userId.ToString())
                .FirstOrDefaultAsync();

            if (globalPermissions != null)
            {
                privileges["NewForms"] = globalPermissions.CreateForms;
                privileges["NewThemes"] = globalPermissions.CreateThemes;
            }

            return privileges;
        }
    }
}
