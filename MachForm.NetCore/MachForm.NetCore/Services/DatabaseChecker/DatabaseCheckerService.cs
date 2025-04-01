using Microsoft.EntityFrameworkCore;

namespace MachForm.NetCore.Services.DatabaseChecker;

public class DatabaseCheckerService : IDatabaseCheckerService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseCheckerService> _logger;

    public DatabaseCheckerService(ApplicationDbContext context, ILogger<DatabaseCheckerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> DatabaseExistsAndMigratedAsync()
    {
        try
        {
            // بررسی وجود دیتابیس
            if (!await _context.Database.CanConnectAsync())
                return false;

            // بررسی وجود جدول MigrationHistory
            var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();
            return appliedMigrations.Any();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking database status");
            return false;
        }
    }

    public async Task<bool> CanConnectToDatabaseAsync()
    {
        try
        {
            return await _context.Database.CanConnectAsync();
        }
        catch
        {
            return false;
        }
    }
}