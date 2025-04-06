namespace MachForm.NetCore.Models.Forms;

public class FormViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public string DisabledMessage { get; set; }
    public int ThemeId { get; set; }
    public bool ApprovalEnable { get; set; }
    public int TodayEntries { get; set; }
    public string LatestEntry { get; set; }
    public DateTime? LastEntryDate { get; set; }
    public int TotalEntries { get; set; }
    public string CreatedBy { get; set; }
    public List<string> Tags { get; set; }
    public int PageNumber { get; set; }
}