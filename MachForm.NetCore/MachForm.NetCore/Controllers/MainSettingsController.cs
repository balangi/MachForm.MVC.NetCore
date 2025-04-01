using MachForm.NetCore.Models.MainSettings;
using MachForm.NetCore.Services.MainSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MachForm.NetCore.Controllers;

// main_settings.php
[Authorize(Roles = "Administrator")]
public class MainSettingsController : Controller
{
    private readonly IMainSettingsService _settingsService;
    private readonly ILogger<MainSettingsController> _logger;

    public MainSettingsController(
        IMainSettingsService settingsService,
        ILogger<MainSettingsController> logger)
    {
        _settingsService = settingsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var model = await _settingsService.GetMainSettingsAsync();
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading system settings");
            TempData["ErrorMessage"] = "Error loading system settings";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(MainSettingsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Reload dropdown data
            model.AvailableThemes = await _settingsService.GetCustomThemesAsync();
            model.BuiltInThemes = await _settingsService.GetBuiltInThemesAsync();
            model.Forms = await _settingsService.GetFormsListAsync();

            return View(model);
        }

        try
        {
            await _settingsService.SaveMainSettingsAsync(model);
            TempData["SuccessMessage"] = "System settings have been saved successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving system settings");
            TempData["ErrorMessage"] = "Error saving system settings";

            // Reload dropdown data
            model.AvailableThemes = await _settingsService.GetCustomThemesAsync();
            model.BuiltInThemes = await _settingsService.GetBuiltInThemesAsync();
            model.Forms = await _settingsService.GetFormsListAsync();

            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExportForm(int formId)
    {
        try
        {
            var exportData = await _settingsService.ExportFormAsync(formId);
            return File(exportData, "application/json", $"form_export_{formId}.json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting form");
            TempData["ErrorMessage"] = "Error exporting form";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportForm(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            TempData["ErrorMessage"] = "Please select a file to import";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            var formId = await _settingsService.ImportFormAsync(stream.ToArray());

            TempData["SuccessMessage"] = $"Form imported successfully! Form ID: {formId}";
            return RedirectToAction("Edit", "Forms", new { id = formId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing form");
            TempData["ErrorMessage"] = "Error importing form. The file may be corrupted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
