using System.ComponentModel.DataAnnotations;

namespace MachForm.NetCore.Models.MainSettings;

public class SecuritySettings
{
    public bool EnforceTwoStepVerification { get; set; }
    public bool EnableIpRestriction { get; set; }

    [Display(Name = "IP Whitelist")]
    public string IpWhitelist { get; set; }

    public bool EnableAccountLocking { get; set; }

    [Range(1, 1440)]
    public int AccountLockPeriod { get; set; } = 30;

    [Range(1, 20)]
    public int AccountLockMaxAttempts { get; set; } = 6;

    public bool EnableDataRetention { get; set; }

    [Range(1, 120)]
    public int DataRetentionPeriod { get; set; } = 12;

    [Display(Name = "reCAPTCHA Site Key")]
    public string RecaptchaSiteKey { get; set; }

    [Display(Name = "reCAPTCHA Secret Key")]
    public string RecaptchaSecretKey { get; set; }
}
