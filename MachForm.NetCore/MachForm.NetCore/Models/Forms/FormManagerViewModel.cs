using MachForm.NetCore.Models.Folders;

namespace MachForm.NetCore.Models.Forms;

public class FormManagerViewModel
{
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
}
