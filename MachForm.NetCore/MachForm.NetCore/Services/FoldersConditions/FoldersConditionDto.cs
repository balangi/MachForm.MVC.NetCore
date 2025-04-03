using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MachForm.NetCore.Services.FoldersConditions;

public class FoldersConditionDto
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FolderId { get; set; }
    public string ElementName { get; set; } = "";
    public string RuleCondition { get; set; } = "is";
    [AllowNull]
    public string RuleKeyword { get; set; } = null;
}
