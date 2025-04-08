namespace MachForm.NetCore.Services.Permissions
{
    public interface IPermissionService
    {
        Task<Dictionary<string, bool>> GetUserPrivileges(string userId);
    }
}
