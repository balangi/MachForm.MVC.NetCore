using MachForm.NetCore.Models.Account;

namespace MachForm.NetCore.Services.Auth;

public interface IUserService
{
    List<UserDto> GetAll();
    UserDto GetItem(int id);
    Task<LockoutRecordDto> GetByUserId(string userId);
    int AllCount();
    int EnableCount();
    Task<bool> DatabaseTablesExist();
    Task<bool> IsIpWhitelisted(string ipAddress);
    Task RunCronJobs(bool forceRun = false);
    Task CreateDefaultFolder(string userId);
    Task UpdateLastLoginInfo(string userId, string ipAddress);
    Task<bool> TwoStepVerificationRequired(string userId);
    string GetUserId();
}
