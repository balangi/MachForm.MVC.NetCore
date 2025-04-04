using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MachForm.NetCore.Models.MainSettings;

public class SettingDto
{
    [Key]
    public int Id { get; set; }
    public string SmtpEnable { get; set; } = "0";
    public string SmtpHost { get; set; } = "localhost";
    public int SmtpPort { get; set; } = 25;
    public string SmtpAuth { get; set; } = "0";
    [AllowNull]
    public string? SmtpUsername { get; set; } = null;
    [AllowNull]
    public string? SmtpPassword { get; set; } = null;
    public string SmtpSecure { get; set; } = "0";
    public string UploadDir { get; set; } = "./data";
    public string DataDir { get; set; } = "./data";
    public string DefaultFromName { get; set; } = "MachForm";
    [AllowNull]
    public string? DefaultFromEmail { get; set; } = null;
    public int DefaultFormThemeId { get; set; } = 0;
    [AllowNull]
    public string? BaseUrl { get; set; } = null;
    public int FormManagerMaxRows { get; set; } = 10;
    [AllowNull]
    public string? AdminImageUrl { get; set; } = null;
    public bool DisableMachformLink { get; set; }
    public int DisablePdfLink { get; set; } = 0;
    [AllowNull]
    public string? CustomerId { get; set; } = null;
    [AllowNull]
    public string? CustomerName { get; set; } = null;
    [AllowNull]
    public string? LicenseKey { get; set; } = null;
    public string MachformVersion { get; set; } = "3.0";
    [AllowNull]
    public string? AdminTheme { get; set; } = null;
    public int EnforceTsv { get; set; } = 0;
    public bool EnableIpRestriction { get; set; }
    [AllowNull]
    public string? IpWhitelist { get; set; }
    public bool EnableAccountLocking { get; set; }
    public int AccountLockPeriod { get; set; } = 30;
    public int AccountLockMaxAttempts { get; set; } = 6;
    [AllowNull]
    public string? RecaptchaSiteKey { get; set; } = null;
    [AllowNull]
    public string? RecaptchaSecretKey { get; set; } = null;
    [AllowNull]
    public string? GoogleapiClientid { get; set; } 
    [AllowNull]
    public string? GoogleapiClientsecret { get; set; }
    public string LdapEnable { get; set; } = "0";
    public LdapType LdapType { get; set; } = (LdapType)1;
    [AllowNull]
    public string? LdapHost { get; set; } = null;
    public int LdapPort { get; set; } = 389;
    public LdapEncryption LdapEncryption { get; set; } = (LdapEncryption)1;
    [AllowNull]
    public string? LdapBasedn { get; set; } = null;
    [AllowNull]
    public string? LdapAccountSuffix { get; set; } = null;
    [AllowNull]
    public string? LdapRequiredGroup { get; set; } = null;
    public string LdapExclusive { get; set; } = "0";
    [AllowNull]
    public string? Timezone { get; set; } = null;
    public string EnableDataRetention { get; set; } = "0";
    public DataRetentionPeriod DataRetentionPeriod { get; set; } = (DataRetentionPeriod)38;
    public string CaptchaPublicKey { get; set; }
    public string CaptchaPrivateKey { get; set; }
}

public enum LdapType
{
    Ad = 1,
    Openldap = 2,
}
public enum LdapEncryption
{
    None = 1,
    Ssl = 2,
    Tls = 3,
}
public enum DataRetentionPeriod
{
    Months = 38,
}