using System.ComponentModel.DataAnnotations;

namespace MachForm.NetCore.Models.Account;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Username or Email")]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }
    public string CsrfToken { get; set; }
}
