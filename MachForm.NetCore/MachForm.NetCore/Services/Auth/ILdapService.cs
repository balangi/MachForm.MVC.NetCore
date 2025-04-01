namespace MachForm.NetCore.Services.Auth;

public interface ILdapService
{
    Task<(bool success, string error, LdapUserInfo userInfo)> Authenticate(string username, string password);
}
