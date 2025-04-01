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

    public InstallController(ApplicationDbContext context, ILogger<InstallController> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SetupDatabase()
    {
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

    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }
}
