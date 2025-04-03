using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MachForm.NetCore.Models.Account;
using MachForm.NetCore.Models.Forms;

namespace MachForm.NetCore.Models.Permissions;

public class PermissionDto
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public int FormId { get; set; }

    // اضافه کردن خصوصیات مجوزها
    public bool EditForm { get; set; }
    public bool EditEntries { get; set; }
    public bool ViewEntries { get; set; }
    public bool EditReport { get; set; }
    public bool Administer { get; set; }
    public bool CreateForms { get; set; }
    public bool CreateThemes { get; set; }

    [ForeignKey("UserId")]
    public virtual UserDto User { get; set; }

    [ForeignKey("FormId")]
    public virtual FormDto Form { get; set; }
}
