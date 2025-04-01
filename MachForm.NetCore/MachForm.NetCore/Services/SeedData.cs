using MachForm.NetCore.Models.Account;
using Microsoft.AspNetCore.Identity;


//using MachForm.Web.Models;
//using MachForm.Web.Models.Identity;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MachForm.NetCore.Models.MainSettings;

namespace MachForm.NetCore.Services;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<UserDto>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    //    var logger = services.GetRequiredService<ILogger<SeedData>>();

        try
        {
            // 1. مطمئن شویم دیتابیس ایجاد شده است
            await context.Database.MigrateAsync();

            // 2. ایجاد نقش‌های سیستم
            await SeedRoles(roleManager);

            // 3. ایجاد کاربر ادمین
            await SeedAdminUser(userManager);

            // 4. ایجاد داده‌های اولیه دیگر
            await SeedInitialData(context);

       //     logger.LogInformation("Seed data completed successfully.");
        }
        catch (Exception ex)
        {
       //     logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Admin", "Manager", "User", "Guest" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task SeedAdminUser(UserManager<UserDto> userManager)
    {
        // بررسی وجود کاربر ادمین
        if (await userManager.FindByEmailAsync("admin@example.com") == null)
        {
            var adminUser = new UserDto
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true,
                FullName = "مدیر سیستم",
                PhoneNumber = "09123456789",
                PhoneNumberConfirmed = true,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            var createAdmin = await userManager.CreateAsync(adminUser, "Admin@1234");
            if (createAdmin.Succeeded)
            {
                // اختصاص نقش به کاربر ادمین
                await userManager.AddToRoleAsync(adminUser, "Admin");
                await userManager.AddToRoleAsync(adminUser, "User");
            }
        }
    }

    private static async Task SeedInitialData(ApplicationDbContext context)
    {
        // ایجاد تنظیمات سیستم
        if (!context.MainSettings.Any())
        {
            var systemSettings = new MainSettingDto
            {
                SiteTitle = "سامانه مدیریت MachForm",
                SiteDescription = "سیستم مدیریت فرم‌ها",
                DefaultTheme = "light",
                MaintenanceMode = false,
                //EmailSettings = new EmailSettings
                //{
                //    SmtpServer = "smtp.example.com",
                //    SmtpPort = 587,
                //    SmtpUsername = "admin@example.com",
                //    SmtpPassword = "password",
                //    FromEmail = "noreply@example.com",
                //    FromName = "سامانه MachForm"
                //},
                CreatedDate = DateTime.Now
            };

            await context.MainSettings.AddAsync(systemSettings);
        }

        // ایجاد چند فرم نمونه
        //if (!context.Forms.Any())
        //{
        //    var sampleForms = new List<Form>
        //    {
        //        new Form
        //        {
        //            Title = "فرم تماس با ما",
        //            Description = "فرم ارتباط با پشتیبانی",
        //            IsActive = true,
        //            CreatedDate = DateTime.Now,
        //            Fields = new List<FormField>
        //            {
        //                new FormField { Name = "نام کامل", FieldType = "text", IsRequired = true, Order = 1 },
        //                new FormField { Name = "ایمیل", FieldType = "email", IsRequired = true, Order = 2 },
        //                new FormField { Name = "موضوع", FieldType = "text", IsRequired = true, Order = 3 },
        //                new FormField { Name = "پیام", FieldType = "textarea", IsRequired = true, Order = 4 }
        //            }
        //        },
        //        new Form
        //        {
        //            Title = "فرم نظرسنجی",
        //            Description = "نظرسنجی از کاربران",
        //            IsActive = true,
        //            CreatedDate = DateTime.Now,
        //            Fields = new List<FormField>
        //            {
        //                new FormField { Name = "سن", FieldType = "number", IsRequired = false, Order = 1 },
        //                new FormField { Name = "جنسیت", FieldType = "radio", Options = "مرد,زن", IsRequired = false, Order = 2 },
        //                new FormField { Name = "نظر شما", FieldType = "textarea", IsRequired = true, Order = 3 }
        //            }
        //        }
        //    };

        //    await context.Forms.AddRangeAsync(sampleForms);
        //}

        await context.SaveChangesAsync();
    }
}