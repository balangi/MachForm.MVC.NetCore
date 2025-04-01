namespace MachForm.NetCore.Services.DatabaseChecker;

public interface IDatabaseCheckerService
{
    Task<bool> DatabaseExistsAndMigratedAsync();
    Task<bool> CanConnectToDatabaseAsync();
}