
namespace MachForm.NetCore.Models.MainSettings;

public class MainSettingDto
{
    public DateTime LastCronRun { get; internal set; }
    public bool EnableDataRetention { get; internal set; }
    public int DataRetentionPeriod { get; internal set; }
    public string SiteTitle { get; internal set; }
    public string SiteDescription { get; internal set; }
    public string DefaultTheme { get; internal set; }
    public bool MaintenanceMode { get; internal set; }
    //public EmailSettings EmailSettings { get; internal set; }
    public DateTime CreatedDate { get; internal set; }
}