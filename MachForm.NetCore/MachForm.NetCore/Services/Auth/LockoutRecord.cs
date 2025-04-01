using System.ComponentModel.DataAnnotations;
using MachForm.NetCore.Models.Account;

namespace MachForm.NetCore.Services.Auth;

public class LockoutRecordDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public DateTime LockoutEnd { get; set; }

    [Required]
    public DateTime LockoutStart { get; set; }

    [Required]
    public string Reason { get; set; }

    [Required]
    public string IpAddress { get; set; }

    // Navigation property
    public UserDto User { get; set; }
}
