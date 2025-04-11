using MachForm.NetCore.Models.Account;
using MachForm.NetCore.Models.ElementOptions;
using MachForm.NetCore.Models.Elements;
using MachForm.NetCore.Models.Folders;
using MachForm.NetCore.Models.FormLocks;
using MachForm.NetCore.Models.Forms;
using MachForm.NetCore.Models.FormSorts;
using MachForm.NetCore.Models.FormStats;
using MachForm.NetCore.Models.FormThemes;
using MachForm.NetCore.Models.MainSettings;
using MachForm.NetCore.Models.Permissions;
using MachForm.NetCore.Models.Themes;
using MachForm.NetCore.Services.Auth;
using MachForm.NetCore.Services.FoldersConditions;
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

    public virtual DbSet<ElementOptionDto> ElementOptions { get; set; } = null!;
    public virtual DbSet<FolderDto> Folders { get; set; } = null!;
    public virtual DbSet<FoldersConditionDto> FoldersConditions { get; set; } = null!;
    public virtual DbSet<FormDto> Forms { get; set; } = null!;
    public virtual DbSet<FormElementDto> FormElements { get; set; } = null!;
    public virtual DbSet<FormLockDto> FormLocks { get; set; } = null!;
    public virtual DbSet<FormThemeDto> FormThemes { get; set; } = null!;
    public virtual DbSet<FormSortDto> FormSorts { get; set; } = null!;
    public virtual DbSet<FormStatDto> FormStats { get; set; } = null!;
    public virtual DbSet<LogDto> Logs { get; set; } = null!;
    public virtual DbSet<LockoutRecordDto> LockoutRecords { get; set; } = null!;
    public virtual DbSet<IpWhitelistDto> IpWhitelists { get; set; } = null!;
    public virtual DbSet<MainSettingDto> MainSettings { get; set; } = null!;
    public virtual DbSet<PermissionDto> Permissions { get; set; } = null!;
    //public virtual DbSet<MenuDto> Menus { get; set; } = null!;
    public virtual DbSet<RoleDto> Roles { get; set; } = null!;
    public virtual DbSet<SettingDto> Settings { get; set; } = null!;
    public virtual DbSet<ThemeDto> Themes { get; set; } = null!;
    public virtual DbSet<GlobalPermissionDto> GlobalPermissions { get; set; } = null!;
    public virtual DbSet<UserDto> Users { get; set; } = null!;
    public virtual DbSet<UserClaimDto> UserClaims { get; set; } = null!;
    public virtual DbSet<UserRoleDto> UserRoles { get; set; } = null!;
    public virtual DbSet<UserTokenDto> UserTokens { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}