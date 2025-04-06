using System.ComponentModel.DataAnnotations;
using MachForm.NetCore.Models.Account;

namespace MachForm.NetCore.Models.Folders;

public class FolderDto
{
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }
    public int Position { get; set; }
    public string Name { get; set; } = "";
    public bool Selected { get; set; }
    public string RuleAllAny { get; set; } = "all";

    public virtual UserDto User { get; set; }
}
