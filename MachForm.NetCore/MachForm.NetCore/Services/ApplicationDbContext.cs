using MachForm.NetCore.Models.Account;
using MachForm.NetCore.Models.Folders;
using MachForm.NetCore.Models.Forms;
using MachForm.NetCore.Models.MainSettings;
using MachForm.NetCore.Models.Themes;
using MachForm.NetCore.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MachForm.NetCore.Services;

public class ApplicationDbContext: IdentityDbContext<UserDto,
    RoleDto, string, IdentityUserClaim<string>,
    UserRoleDto, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<FormDto> Forms { get; set; } = null!;
    public virtual DbSet<FolderDto> Folders { get; set; } = null!;
    public virtual DbSet<LogDto> Logs { get; set; } = null!;
    public virtual DbSet<LockoutRecordDto> LockoutRecords { get; set; } = null!;
    public virtual DbSet<IpWhitelistDto> IpWhitelists { get; set; } = null!;
    public virtual DbSet<MainSettingDto> MainSettings { get; set; } = null!;
    //public virtual DbSet<MenuDto> Menus { get; set; } = null!;
    public virtual DbSet<RoleDto> Roles { get; set; } = null!;
    public virtual DbSet<ThemeDto> Themes { get; set; } = null!;
    public virtual DbSet<UserDto> Users { get; set; } = null!;
    public virtual DbSet<UserClaimDto> UserClaims { get; set; } = null!;
    public virtual DbSet<UserRoleDto> UserRoles { get; set; } = null!;
    public virtual DbSet<UserTokenDto> UserTokens { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}