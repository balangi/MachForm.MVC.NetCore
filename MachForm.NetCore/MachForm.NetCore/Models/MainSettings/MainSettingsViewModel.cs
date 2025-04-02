using MachForm.NetCore.Models.Account;
using MachForm.NetCore.Models.Themes;

namespace MachForm.NetCore.Models.MainSettings;

public class MainSettingsViewModel
{
    public SmtpSettings Smtp { get; set; }
    public LdapSettings Ldap { get; set; }
    public SecuritySettings Security { get; set; }
    public GeneralSettingDto General { get; set; }
    public List<ThemeDto> AvailableThemes { get; set; }
    public List<ThemeDto> BuiltInThemes { get; set; }
    public List<FormInfo> Forms { get; set; }
}

