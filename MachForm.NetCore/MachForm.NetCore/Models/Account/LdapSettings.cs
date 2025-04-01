using System.ComponentModel.DataAnnotations;

namespace MachForm.NetCore.Models.Account;

public class LdapSettings
{
    public bool Enabled { get; set; }
    public bool Exclusive { get; set; }

    [Required]
    public string Type { get; set; } // 'ad' or 'openldap'

    [Required]
    [Display(Name = "LDAP Host")]
    public string Host { get; set; }

    [Required]
    [Range(1, 65535)]
    public int Port { get; set; } = 389;

    [Required]
    public string Encryption { get; set; } // 'ssl', 'tls' or none

    [Required]
    [Display(Name = "Base DN")]
    public string BaseDn { get; set; }

    [Display(Name = "Account Suffix")]
    public string AccountSuffix { get; set; }

    [Display(Name = "Required Groups")]
    public string RequiredGroup { get; set; }
}
