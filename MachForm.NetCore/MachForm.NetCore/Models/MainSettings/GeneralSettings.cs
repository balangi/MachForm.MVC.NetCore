using System.ComponentModel.DataAnnotations;

namespace MachForm.NetCore.Models.MainSettings;

public class GeneralSettings
{
    [Required]
    public string TimeZone { get; set; }

    [Required]
    public string AdminTheme { get; set; }

    [Url, Display(Name = "Admin Panel Image URL")]
    public string AdminImageUrl { get; set; }

    [Display(Name = "Default From Name")]
    public string DefaultFromName { get; set; }

    [EmailAddress, Display(Name = "Default From Email")]
    public string DefaultFromEmail { get; set; }

    [Url, Display(Name = "Base URL")]
    public string BaseUrl { get; set; }

    [Display(Name = "Upload Directory")]
    public string UploadDir { get; set; }

    [Display(Name = "Default Form Theme")]
    public int DefaultFormThemeId { get; set; }

    [Range(10, 500), Display(Name = "Form Manager Max Rows")]
    public int FormManagerMaxRows { get; set; } = 50;

    public bool DisableMachFormLink { get; set; }
    public bool DisablePdfLinks { get; set; }

    [Display(Name = "Google API Client ID")]
    public string GoogleApiClientId { get; set; }

    [Display(Name = "Google API Client Secret")]
    public string GoogleApiClientSecret { get; set; }
}
