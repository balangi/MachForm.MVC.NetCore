using Microsoft.EntityFrameworkCore;

namespace MachForm.NetCore.Services.Permissions
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _dbContext;

        public PermissionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Dictionary<string, bool>> GetUserPrivileges(string userId)
        {
            var privileges = new Dictionary<string, bool>
            {
                ["priv_administer"] = false,
                ["priv_new_forms"] = false,
                ["priv_new_themes"] = false,
                ["priv_edit_form"] = false,
                ["priv_edit_entries"] = false,
                ["priv_view_entries"] = false,
                ["priv_edit_report"] = false
            };

            // بررسی مجوزهای عمومی کاربر
            var userPermissions = await _dbContext.Permissions
                .Where(up => up.UserId == userId.ToString())
                .ToListAsync();

            // اگر کاربر دسترسی ادمین دارد، تمام مجوزها را true می‌کنیم
            if (userPermissions.Any(up => up.Administer))
            {
                foreach (var key in privileges.Keys.ToList())
                {
                    privileges[key] = true;
                }
                return privileges;
            }

            // بررسی مجوزهای اختصاصی
            foreach (var permission in userPermissions)
            {
                privileges["priv_edit_form"] |= permission.EditForm;
                privileges["priv_edit_entries"] |= permission.EditEntries;
                privileges["priv_view_entries"] |= permission.ViewEntries;
                privileges["priv_edit_report"] |= permission.EditReport;
            }

            // بررسی مجوزهای عمومی (غیر وابسته به فرم خاص)
            var globalPermissions = await _dbContext.UserGlobalPermissions
                .Where(ugp => ugp.UserId == userId.ToString())
                .FirstOrDefaultAsync();

            if (globalPermissions != null)
            {
                privileges["priv_new_forms"] = globalPermissions.CreateForms;
                privileges["priv_new_themes"] = globalPermissions.CreateThemes;
            }

            return privileges;
        }

    }
}
