using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MachForm.NetCore.Models.Account;

namespace MachForm.NetCore.Models.Permissions;

public class UserGlobalPermissionDto
{
    [Key]
    public int Id { get; set; }

    public string UserId { get; set; }
    public bool CreateForms { get; set; }
    public bool CreateThemes { get; set; }

    [ForeignKey("UserId")]
    public virtual UserDto User { get; set; }
}
