#nullable disable

using Microsoft.AspNetCore.Identity;

namespace MachForm.NetCore.Models.Account;

public class RoleDto : IdentityRole<string>
{ 
    public string Description { get; set; }
    public ICollection<UserRoleDto> UserRoles { get; set; }
}
