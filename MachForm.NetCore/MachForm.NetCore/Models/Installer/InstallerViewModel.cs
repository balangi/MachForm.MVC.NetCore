using System.ComponentModel.DataAnnotations;

namespace MachForm.NetCore.Models.Installer;

public class InstallerViewModel
{
    public bool IsDotNetVersionPassed { get; set; }
    public string PreInstallError { get; set; }
    public bool InstallationSuccess { get; set; }
    public bool InstallationHasError { get; set; }
    public string InstallationError { get; set; }

    [Required]
    [EmailAddress]
    public string AdminUsername { get; set; }

    [Required]
    public string LicenseKey { get; set; }
}
