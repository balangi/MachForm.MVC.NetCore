using System.ComponentModel.DataAnnotations;

namespace MachForm.NetCore.Models.FormSorts;

public class FormSortDto
{
    [Key]
    public int FormId { get; set; }
    public string UserId { get; set; }
    public string SortBy { get; set; } = "";
}
