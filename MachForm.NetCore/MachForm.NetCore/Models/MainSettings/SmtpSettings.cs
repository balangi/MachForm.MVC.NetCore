using System.ComponentModel.DataAnnotations;

namespace MachForm.NetCore.Models.MainSettings;

public class SmtpSettings
{
    public bool Enabled { get; set; }

    [Required]
    [Display(Name = "SMTP Server")]
    public string Host { get; set; }

    private object Display(object value)
    {
        throw new NotImplementedException();
    }

    public bool UseAuthentication { get; set; }
    public bool UseSsl { get; set; }

    [Display(Name = "Username")]
    public string Username { get; set; }

    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required]
    [Range(1, 65535)]
    public int Port { get; set; } = 25;
}
