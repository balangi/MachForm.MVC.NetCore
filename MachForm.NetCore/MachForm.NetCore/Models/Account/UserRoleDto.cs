#nullable disable

using Microsoft.AspNetCore.Identity;

namespace MachForm.NetCore.Models.Account;

public class UserRoleDto : IdentityUserRole<string>
{ 
    public virtual UserDto User { get; set; }
    public virtual RoleDto Role { get; set; }
}
