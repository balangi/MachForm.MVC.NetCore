using MachForm.NetCore.Models.Account;
using MachForm.NetCore.Models.MainSettings;
using MachForm.NetCore.Services;
using MachForm.NetCore.Services.Auth;
using MachForm.NetCore.Services.DatabaseChecker;
using MachForm.NetCore.Services.MainSettings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<UserDto>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
    .AddRoles<RoleDto>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.Configure<LdapSettings>(builder.Configuration.GetSection("MainSettings:Ldap"));
//builder.Services.Configure<MachFormSettings>(builder.Configuration.GetSection("MachFormSettings"));
builder.Services.Configure<MainSettingsViewModel>(builder.Configuration.GetSection("MainSettings"));

builder.Services.AddScoped<IDatabaseCheckerService, DatabaseCheckerService>(); 
builder.Services.AddScoped<ILdapService, LdapService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMainSettingsService, MainSettingsService>();
builder.Services.AddAutoMapper(typeof(Program));


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