using MachForm.NetCore.Models.MainSettings;
using MachForm.NetCore.Models.Themes;

namespace MachForm.NetCore.Services.MainSettings;

public interface IMainSettingsService
{
    Task<MainSettingsViewModel> GetMainSettingsAsync();
    Task SaveMainSettingsAsync(MainSettingsViewModel model);
    Task<List<ThemeDto>> GetCustomThemesAsync();
    Task<List<ThemeDto>> GetBuiltInThemesAsync();
    Task<List<FormInfo>> GetFormsListAsync();
    Task<byte[]> ExportFormAsync(int formId);
    Task<int> ImportFormAsync(byte[] formData);
}
