#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MachForm.NetCore.Models.Forms;
using MachForm.NetCore.Services.Auth;
using Microsoft.AspNetCore.Identity;

namespace MachForm.NetCore.Models.Account;

public class UserDto : IdentityUser
{ 
    public ICollection<UserRoleDto> UserRoles { get; set; }

    //[InverseProperty(nameof(NewsDto.UserInfo))]
    //public ICollection<NewsDto> News { get; set; }

    //[InverseProperty(nameof(SponsorDto.UserInfo))]
    //public ICollection<SponsorDto> Sponsors { get; set; }

    //[InverseProperty(nameof(GalleryDto.UserInfo))]
    //public ICollection<GalleryDto> Galleries { get; set; }

    //[InverseProperty(nameof(FormDto.FormInfo))]
    //public ICollection<FormDto> Forms { get; set; }

    public string FullName { get; set; }
    public bool IsActive { get; set; }
    public UserStatus Status { get; internal set; }
    internal UserPrivileges Privileges { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? DeactivatedDate { get; set; }
    public string DeactivationReason { get; set; }
    public AccountType AccountType { get; set; }
    public string LastIpAddress { get; internal set; }
    public int LoginAttemptCount { get; internal set; }
    public DateTime? LoginAttemptDate { get; internal set; }

    [Required]
    public bool TwoFactorEnabled { get; set; }

    public string TwoFactorSecret { get; set; }

    public bool IsTwoFactorEnforced { get; set; }
    public bool IsAdmin { get; internal set; }
    public DateTime CreatedAt { get; internal set; }
    public int FoldersPinned { get; set; } = 0;
}
