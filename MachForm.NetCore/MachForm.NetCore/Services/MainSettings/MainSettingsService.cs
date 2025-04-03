using MachForm.NetCore.Models.Account;
using MachForm.NetCore.Models.MainSettings;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using MachForm.NetCore.Models.Themes;
using Microsoft.EntityFrameworkCore;
using MachForm.NetCore.Models.Forms;
using AutoMapper;
using MachForm.NetCore.Models.FormStats;

namespace MachForm.NetCore.Services.MainSettings;

public class MainSettingsService : IMainSettingsService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IOptionsSnapshot<MainSettingsViewModel> _settings;
    private readonly IMapper _mapper;

    public MainSettingsService(
        ApplicationDbContext context,
        IOptionsSnapshot<MainSettingsViewModel> settings,
        IMapper mapper)
    {
        _dbContext = context;
        _settings = settings;
        _mapper = mapper;
    }

    public async Task<MainSettingsViewModel> GetMainSettingsAsync()
    {
        var model = new MainSettingsViewModel
        {
            Smtp = _mapper.Map<SmtpSettings>(_settings.Value.Smtp),
            Ldap = _mapper.Map<LdapSettings>(_settings.Value.Ldap),
            Security = _mapper.Map<SecuritySettings>(_settings.Value.Security),
            General = _mapper.Map<SettingDto>(_settings.Value.General),
            AvailableThemes = await GetCustomThemesAsync(),
            BuiltInThemes = await GetBuiltInThemesAsync(),
            Forms = await GetFormsListAsync()
        };

        return model;
    }

    public async Task SaveMainSettingsAsync(MainSettingsViewModel model)
    {
        // In a real application, you would save to database or configuration store
        // This is a simplified version

        var settings = _settings.Value;
        _mapper.Map(model.Smtp, settings.Smtp);
        _mapper.Map(model.Ldap, settings.Ldap);
        _mapper.Map(model.Security, settings.Security);
        _mapper.Map(model.General, settings.General);

        // Save to configuration store or database
        await Task.CompletedTask;
    }

    public async Task<List<ThemeDto>> GetCustomThemesAsync()
    {
        return await _dbContext.Themes
            .Where(t => !t.IsBuiltIn && t.Status == ThemeStatus.Active)
            .OrderBy(t => t.Name)
            .Select(t => new ThemeDto { Id = t.Id, Name = t.Name })
            .ToListAsync();
    }

    public async Task<List<ThemeDto>> GetBuiltInThemesAsync()
    {
        return await _dbContext.Themes
            .Where(t => t.IsBuiltIn && t.Status == ThemeStatus.Active)
            .OrderBy(t => t.Name)
            .Select(t => new ThemeDto { Id = t.Id, Name = t.Name })
            .ToListAsync();
    }

    public async Task<List<FormInfo>> GetFormsListAsync()
    {
        return await _dbContext.Forms
            //.Where(f => f.Status == FormStatus.Active || f.Status == FormStatus.Inactive)
            .Where(f => f.IsActive == Convert.ToBoolean((int)FormStatus.Active) || f.IsActive == Convert.ToBoolean((int)FormStatus.Inactive))
            .OrderBy(f => f.Name)
            .Select(f => new FormInfo
            {
                Id = f.Id,
                Name = string.IsNullOrEmpty((string?)f.Name) ? $"-Untitled Form- (#{f.Id})" : $"{f.Name} (#{f.Id})"
            })
            .ToListAsync();
    }

    public async Task<byte[]> ExportFormAsync(int formId)
    {
        var form = await _dbContext.Forms
            .Include(f => f.FormElementInfo)
            .FirstOrDefaultAsync(f => f.Id == formId);

        if (form == null)
            throw new ArgumentException("Form not found");

        var exportData = JsonSerializer.Serialize(form);
        return Encoding.UTF8.GetBytes(exportData);
    }

    public async Task<int> ImportFormAsync(byte[] formData)
    {
        var json = Encoding.UTF8.GetString(formData);
        var form = JsonSerializer.Deserialize<FormDto>(json);

        if (form == null)
            throw new InvalidOperationException("Invalid form data");

        // Reset ID and other properties for new form
        form.Id = 0;
        form.CreatedDate = DateTime.UtcNow;
        //form.Status = FormStatus.Active;
        form.IsActive = Convert.ToBoolean((int)FormStatus.Active);

        //foreach (var element in form.Elements)
        //{
        //    element.Id = 0;
        //    element.FormId = 0;
        //}

        _dbContext.Forms.Add(form);
        await _dbContext.SaveChangesAsync();

        return form.Id;
    }
}