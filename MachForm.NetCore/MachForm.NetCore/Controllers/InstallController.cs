using System.Text.RegularExpressions;
using MachForm.NetCore.Helpers;
using MachForm.NetCore.Models.Account;
using MachForm.NetCore.Models.Installer;
using MachForm.NetCore.Models.MainSettings;
using MachForm.NetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MachForm.NetCore.Controllers;

[AllowAnonymous]
public class InstallController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<InstallController> _logger;
    private readonly IFileHelper _fileHelper;

    public InstallController(ApplicationDbContext context, ILogger<InstallController> logger, IFileHelper fileHelper)
    {
        _dbContext = context;
        _logger = logger;
        _fileHelper = fileHelper;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new InstallerViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> SetupDatabase()
    {
        var model = new InstallerViewModel();

        // 1. Check PHP version (in C# we'll check .NET version)
        model.IsDotNetVersionPassed = Environment.Version.Major >= 5; // .NET 5+ requirement

        if (!model.IsDotNetVersionPassed)
        {
            model.PreInstallError = "dotnet_version_insufficient";
            return View(model);
        }

        // 2. Check connection to Database
        try
        {
            if (!_dbContext.Database.CanConnect())
            {
                model.PreInstallError = $"Error connecting to the database: {_dbContext.Database.GetConnectionString()}";
                return View(model);
            }
        }
        catch (Exception ex)
        {
            model.PreInstallError = $"Error connecting to the database: {ex.Message}";
            return View(model);
        }

        // 3. Check MySQL version (Entity Framework handles this automatically)

        // 4. Check for existing installation
        if (_dbContext.Forms.Any())
        {
            model.PreInstallError = "machform_already_installed";
            return View(model);
        }

        // 5. Check the "data" folder
        var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data");
        if (!Directory.Exists(dataPath))
        {
            try
            {
                Directory.CreateDirectory(dataPath);
            }
            catch
            {
                model.PreInstallError = "data_dir_unwritable";
                return View(model);
            }
        }

        if (!_fileHelper.IsDirectoryWritable(dataPath))
        {
            model.PreInstallError = "data_dir_unwritable";
            return View(model);
        }

        return View(model);

        try
        {
            // ایجاد دیتابیس اگر وجود ندارد
            await _dbContext.Database.EnsureCreatedAsync();

            // اجرای migrations
            await _dbContext.Database.MigrateAsync();

            // ایجاد داده های اولیه

            //var services = scope.ServiceProvider;

            //    await SeedData.Initialize(services);
            //    //await SeedData.Initialize(_dbContext);

            return RedirectToAction("Success");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database setup failed");
            ViewBag.ErrorMessage = "خطا در راه‌اندازی پایگاه داده. لطفاً اطمینان حاصل کنید که اطلاعات اتصال به دیتابیس صحیح است.";
            return View("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Index(InstallerViewModel model)
    {
        if (string.IsNullOrEmpty(model.AdminUsername))
        {
            ModelState.AddModelError("AdminUsername", "Email address is required");
            return View(model);
        }

        if (!IsValidEmail(model.AdminUsername))
        {
            ModelState.AddModelError("AdminUsername", "Please enter a valid email address");
            return View(model);
        }

        if (string.IsNullOrEmpty(model.LicenseKey))
        {
            ModelState.AddModelError("LicenseKey", "License key is required");
            return View(model);
        }

        // Perform installation
        try
        {
            await _dbContext.Database.EnsureCreatedAsync();

            // Create all tables through Entity Framework migrations
            await _dbContext.Database.MigrateAsync();

            // Add default admin user
            var adminUser = new UserDto
            {
                UserName = model.AdminUsername,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("machform"), // Default password
                Email = model.AdminUsername,
                IsAdmin = true,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _dbContext.Users.Add(adminUser);
            await _dbContext.SaveChangesAsync();

            // Add default settings
            var domain = Request.Host.Value.Replace("www.", "");
            var defaultFromEmail = $"no-reply@{domain}";
            var sslSuffix = Request.IsHttps ? "s" : "";
            var baseUrl = $"http{sslSuffix}://{Request.Host.Value}{Request.PathBase}/";
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data");

            var settings = new SettingDto
            {
                DefaultFromEmail = defaultFromEmail,
                BaseUrl = baseUrl,
                LicenseKey = model.LicenseKey,
                UploadDir = uploadDir,
                MachformVersion = "18.1",
                CaptchaPrivateKey = "123",
                CaptchaPublicKey = "456",
            };

            _dbContext.GeneralSettings.Add(settings);
            await _dbContext.SaveChangesAsync();

            // Create themes directory
            var themesPath = Path.Combine(uploadDir, "themes");
            if (!Directory.Exists(themesPath))
            {
                Directory.CreateDirectory(themesPath);
            }

            model.InstallationSuccess = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Installation failed");
            var sep = ex.InnerException != null ? " - " : " ";
            model.InstallationError = $"{ex?.Message}{sep}{ex?.InnerException?.Message}";
            model.InstallationHasError = true;
        }

        return View(model);
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Simple regex for email validation
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private bool IsDirectoryWritable(string path)
    {
        try
        {
            var testFile = Path.Combine(path, "test.txt");
            System.IO.File.WriteAllText(testFile, "test");
            System.IO.File.Delete(testFile);
            return true;
        }
        catch
        {
            return false;
        }
    }

    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }
}
