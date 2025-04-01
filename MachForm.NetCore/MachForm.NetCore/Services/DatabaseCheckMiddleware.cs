using MachForm.NetCore.Services.DatabaseChecker;

namespace MachForm.NetCore.Services;

public class DatabaseCheckMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DatabaseCheckMiddleware> _logger;

    public DatabaseCheckMiddleware(RequestDelegate next, ILogger<DatabaseCheckMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IDatabaseCheckerService dbChecker)
    {
        // از بررسی برای مسیرهای مربوط به نصب و استاتیک فایل‌ها صرف‌نظر می‌کنیم
        if (context.Request.Path.StartsWithSegments("/install") ||
            context.Request.Path.StartsWithSegments("/_framework") ||
            context.Request.Path.StartsWithSegments("/_content") ||
            context.Request.Path.StartsWithSegments("/css") ||
            context.Request.Path.StartsWithSegments("/js") ||
            context.Request.Path.StartsWithSegments("/lib"))
        {
            await _next(context);
            return;
        }

        // بررسی وضعیت دیتابیس
        var dbReady = await dbChecker.DatabaseExistsAndMigratedAsync();

        if (!dbReady)
        {
            _logger.LogWarning("Database not ready, redirecting to installer");
            context.Response.Redirect("/install/index");
            return;
        }

        await _next(context);
    }
}