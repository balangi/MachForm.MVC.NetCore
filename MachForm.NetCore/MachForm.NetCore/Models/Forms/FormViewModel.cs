namespace MachForm.NetCore.Models.Forms;

public class FormViewModel
{
    public int FormId { get; set; }
    public string FormName { get; set; }
    public bool FormActive { get; set; }
    public string FormDisabledMessage { get; set; }
    public int FormThemeId { get; set; }
    public bool FormApprovalEnable { get; set; }
    public int TodayEntries { get; set; }
    public string LatestEntry { get; set; }
    public int TotalEntries { get; set; }
    public string FormCreatedBy { get; set; }
    public List<string> FormTags { get; set; }
    public int PageNumber { get; set; }
}