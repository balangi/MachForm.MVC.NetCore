using System.Security.Claims;

namespace MachForm.NetCore.Models;

public class GlobalModel
{
    public string CurrentNavTab { get; set; }
    public Task<Dictionary<string, bool>> UserPrivilege { get; set; }   
}
