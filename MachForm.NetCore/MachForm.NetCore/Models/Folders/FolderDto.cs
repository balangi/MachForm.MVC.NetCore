using MachForm.NetCore.Entities;

namespace MachForm.NetCore.Models.Folders;

public class FolderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FolderId { get; set; }
    public int Position { get; set; }
    public string Name { get; set; }
    public bool Selected { get; set; }
    public string RuleAllAny { get; set; }

    public virtual User User { get; set; }
}
