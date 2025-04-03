using System.ComponentModel.DataAnnotations;

namespace MachForm.NetCore.Models.FormSorts;

public class FormSortDto
{
    [Key]
    public int FormId { get; set; }
    public int UserId { get; set; }
    public string SortBy { get; set; } = "";
}
