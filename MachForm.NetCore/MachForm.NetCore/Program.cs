using MachForm.NetCore.Helpers;
using MachForm.NetCore.Models.Account;
using MachForm.NetCore.Models.MainSettings;
using MachForm.NetCore.Services;
using MachForm.NetCore.Services.Auth;
using MachForm.NetCore.Services.DatabaseChecker;
using MachForm.NetCore.Services.MainSettings;
using MachForm.NetCore.Services.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<UserDto>(options =>
        {
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;

            // برای دیباگ
            options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
        })
    .AddRoles<RoleDto>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<UserDto, RoleDto>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.Configure<LdapSettings>(builder.Configuration.GetSection("MainSettings:Ldap"));
builder.Services.Configure<MainSettingsViewModel>(builder.Configuration.GetSection("MainSettings"));

builder.Services.AddScoped<IDatabaseCheckerService, DatabaseCheckerService>(); 
builder.Services.AddScoped<IFileHelper, FileHelper>(); 
builder.Services.AddScoped<ISftpFileChecker, SftpFileChecker>(); 
builder.Services.AddScoped<ILdapService, LdapService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IMainSettingsService, MainSettingsService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ISftpClient, SftpClient>(provider =>
    new SftpClient("host", "username", "password"));

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

// غیرفعال کردن موقت اعتبارسنجی کوکی (فقط برای محیط توسعه)
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbChecker = services.GetRequiredService<IDatabaseCheckerService>();
        if (!await dbChecker.CanConnectToDatabaseAsync())
        {
            throw new Exception("Cannot connect to database");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while connecting to the database");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<DatabaseCheckMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ManageForms}/{id?}");
app.MapRazorPages();

app.Run();