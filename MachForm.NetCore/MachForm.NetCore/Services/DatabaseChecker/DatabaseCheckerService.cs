using Microsoft.EntityFrameworkCore;

namespace MachForm.NetCore.Services.DatabaseChecker;

public class DatabaseCheckerService : IDatabaseCheckerService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DatabaseCheckerService> _logger;

    public DatabaseCheckerService(ApplicationDbContext context, ILogger<DatabaseCheckerService> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public async Task<bool> DatabaseExistsAndMigratedAsync()
    {
        try
        {
            // بررسی وجود دیتابیس
            if (!await _dbContext.Database.CanConnectAsync())
                return false;

            // بررسی وجود جدول MigrationHistory
            var appliedMigrations = await _dbContext.Database.GetAppliedMigrationsAsync();
            return appliedMigrations.Count() == 0;
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
            return await _dbContext.Database.CanConnectAsync();
        }
        catch
        {
            return false;
        }
    }
}