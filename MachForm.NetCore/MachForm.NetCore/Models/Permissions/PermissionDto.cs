namespace MachForm.NetCore.Models.Permissions;

public class PermissionDto
{
    public long FormId { get; set; }
    public int UserId { get; set; }
    public int EditForm { get; set; } = 0;
    public int EditReport { get; set; } = 0;
    public int EditEntries { get; set; } = 0;
    public int ViewEntries { get; set; } = 0;

}
