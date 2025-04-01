using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachForm.NetCore.Services.Auth;

[Table("IpWhitelist")]
public class IpWhitelistDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string IpAddress { get; set; }

    [MaxLength(255)]
    public string Description { get; set; }
}
