using System.ComponentModel.DataAnnotations;
using MachForm.NetCore.Models.Account;

namespace MachForm.NetCore.Models.Folders;

public class FolderDto
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FolderId { get; set; }
    public int FolderPosition { get; set; }
    public string FolderName { get; set; } = "";
    public int FolderSelected { get; set; } = 0;
    public string RuleAllAny { get; set; } = "all";

    public virtual UserDto User { get; set; }
}
