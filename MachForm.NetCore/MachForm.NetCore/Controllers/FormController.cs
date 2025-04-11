using MachForm.NetCore.Models;
using MachForm.NetCore.Models.FormLocks;
using MachForm.NetCore.Models.Forms;
using MachForm.NetCore.Models.FormStats;
using MachForm.NetCore.Models.Permissions;
using MachForm.NetCore.Properties;
using MachForm.NetCore.Services;
using MachForm.NetCore.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using static MachForm.NetCore.Models.Forms.FormDto;

namespace MachForm.NetCore.Controllers;

[Authorize]
public class FormController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IUserService _userService;

    public FormController(
        ApplicationDbContext dbContext,
        IAuthorizationService authorizationService,
        IStringLocalizer<Resources> localizer,
        IUserService userService)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
        _localizer = localizer;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id, string unlockHash)
    {
        // بررسی مجوزهای کاربر
        if (!User.Identity.IsAuthenticated)
        {
            TempData["ErrorMessage"] = _localizer["AccessDenied"];
            return RedirectToAction("Login", "Account");
        }

        // اگر فرم جدید است
        if (!id.HasValue)
        {
            if (!await HasPermission("priv_new_forms"))
            {
                TempData["ErrorMessage"] = _localizer["NoPermissionCreateForm"];
                return RedirectToAction("Restricted", "Home");
            }

            return await CreateNewForm();
        }

        // دریافت اطلاعات فرم
        var form = await _dbContext.Forms
            // .Include(f => f.Elements)
            .Include(f => f.FormElementInfo)
                .ThenInclude(e => e.Options)
            .FirstOrDefaultAsync(f => f.Id == id.Value);

        if (form == null)
        {
            TempData["ErrorMessage"] = _localizer["FormNotFound"];
            return RedirectToAction("Index", "Home");
        }

        // بررسی وضعیت فعال بودن فرم
        if (form.Active != 0 && form.Active != 1)
        {
            TempData["ErrorMessage"] = _localizer["InvalidFormStatus"];
            return RedirectToAction("Restricted", "Home");
        }

        // بررسی مجوزهای دسترسی به فرم
        if (!await HasPermission("priv_administer"))
        {
            var userPermissions = await _dbContext.Permissions
                .FirstOrDefaultAsync(p => p.FormId == id.Value && p.UserId == _userService.GetUserId());

            if (userPermissions == null || !userPermissions.EditForm)
            {
                TempData["ErrorMessage"] = _localizer["NoPermissionEditForm"];
                return RedirectToAction("Restricted", "Home");
            }
        }

        // بررسی قفل بودن فرم
        var formLock = await _dbContext.FormLocks
            .FirstOrDefaultAsync(fl => fl.FormId == id.Value && fl.UserId != _userService.GetUserId());

        if (formLock != null && (DateTime.Now - formLock.LockDate).TotalHours < 1 && string.IsNullOrEmpty(unlockHash))
        {
            return RedirectToAction("Locked", "Form", new { id = id.Value });
        }

        // حذف فیلدهای ذخیره نشده قبلی
        await CleanupUnsavedElements(id.Value);

        // قفل کردن فرم برای کاربر فعلی
        await LockFormForCurrentUser(id.Value);

        // آماده سازی داده برای نمایش
        ViewBag.JQueryDataCode = GenerateJQueryDataCode(form);
        ViewBag.IsNewForm = false;
        ViewBag.Markup = await GenerateFormMarkup(form);

        return View(form);
    }

    private Task<bool> HasPermission(string permissionKey)
    {
        // پیاده‌سازی منطق بررسی پرمیشن
        // ...
        return Task.FromResult(false); // Return a default value to ensure all code paths return a value
    }

    private async Task<IActionResult> CreateNewForm()
    {
        // تولید شناسه تصادفی برای فرم جدید
        var lastFormId = await _dbContext.Forms.MaxAsync(f => (int?)f.Id) ?? 10000;
        var newFormId = lastFormId + new Random().Next(100, 1000);

        // ایجاد فرم جدید
        var newForm = new FormDto
        {
            Id = newFormId,
            Name = _localizer["UntitledForm"],
            Description = _localizer["DefaultFormDescription"],
            IsActive = (FormStatus)2, // Draft
            SuccessMessage = _localizer["DefaultSuccessMessage"],
            PageTotal = 1,
            PaginationType = "steps",
            // سایر مقادیر پیش‌فرض
        };

        _dbContext.Forms.Add(newForm);

        // ایجاد پرمیشن برای کاربر فعلی
        _dbContext.Permissions.Add(new PermissionDto
        {
            FormId = newFormId,
            UserId = _userService.GetUserId(),
            EditForm = true,
            EditEntries = true,
            ViewEntries = true
        });

        await _dbContext.SaveChangesAsync();

        // آماده سازی داده برای نمایش
        ViewBag.JQueryDataCode = GenerateJQueryDataCode(newForm);
        ViewBag.IsNewForm = true;
        ViewBag.Markup = await GenerateFormMarkup(newForm);

        return View("Edit", newForm);
    }

    private async Task CleanupUnsavedElements(int formId)
    {
        // حذف عناصر ذخیره نشده (وضعیت 2)
        var unsavedElements = await _dbContext.FormElements
            .Where(e => e.FormId == formId && e.Status == 2)
            .ToListAsync();

        _dbContext.FormElements.RemoveRange(unsavedElements);

        // حذف گزینه‌های ذخیره نشده (live=2)
        var unsavedOptions = await _dbContext.ElementOptions
            //.Where(o => o.Element.FormId == formId && o.Live == 2)
            .Where(o => o.FormId == formId && o.Live == 2)
            .ToListAsync();

        _dbContext.ElementOptions.RemoveRange(unsavedOptions);

        await _dbContext.SaveChangesAsync();
    }

    private async Task LockFormForCurrentUser(int formId)
    {
        // حذف قفل‌های قبلی
        var existingLocks = await _dbContext.FormLocks
            .Where(fl => fl.FormId == formId)
            .ToListAsync();

        _dbContext.FormLocks.RemoveRange(existingLocks);

        // ایجاد قفل جدید
        _dbContext.FormLocks.Add(new FormLockDto
        {
            FormId = formId,
            UserId = _userService.GetUserId(),
            LockDate = DateTime.Now
        });

        await _dbContext.SaveChangesAsync();
    }

    private string GenerateJQueryDataCode(FormDto form)
    {
        // پیاده‌سازی منطق تولید کد jQuery
        // ...
        return string.Empty; // Return a default value to ensure all code paths return a value
    }

    private Task<string> GenerateFormMarkup(FormDto form)
    {
        // پیاده‌سازی منطق تولید HTML فرم
        // ...
        return Task.FromResult(string.Empty); // Return a default value to ensure all code paths return a value
    }
}