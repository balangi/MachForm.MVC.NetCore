using MachForm.NetCore.Models.Folders;
using MachForm.NetCore.Models.Permissions;

namespace MachForm.NetCore.Models.Forms;

public class ManageFormViewModel
{
    public List<FormViewModel> Forms { get; set; } = new();
    public List<FormViewModel> FormList { get; set; }
    public int SelectedFormId { get; set; }
    public bool HighlightSelectedFormId { get; set; }
    public int SelectedFolderId { get; set; }
    public string SelectedFolderName { get; set; }
    public int TotalActiveForms { get; set; }
    public int RowsPerPage { get; set; }
    public int TotalPages { get; set; }
    public List<string> AllTagNames { get; set; }
    public Dictionary<int, string> ThemeList { get; set; }
    public Dictionary<int, string> BuiltInThemes { get; set; }
    public string FormSortByComplete { get; set; }
    public string SortByTitle { get; set; }
    public bool IsFolderPinned { get; set; }
    public List<FolderDto> Folders { get; set; }
    public Dictionary<string, bool> UserPrivileges { get; set; }
    public Dictionary<int, PermissionDto> UserPermissions { get; set; }
    public bool IsAdmin { get; set; }
    public bool CanCreateForms { get; set; } = true;
    public Dictionary<int, string> Themes { get; set; } = new();
}
