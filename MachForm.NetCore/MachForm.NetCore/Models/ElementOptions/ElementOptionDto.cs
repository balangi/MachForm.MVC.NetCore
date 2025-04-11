using System.ComponentModel.DataAnnotations;

namespace MachForm.NetCore.Models.ElementOptions;

public class ElementOptionDto
{
    [Key]
    public int Id { get; set; }
    public int FormId { get; set; } = 0;
    public int ElementId { get; set; } = 0;
    public int OptionId { get; set; } = 0;
    public int Position { get; set; } = 0;
    public string Option { get; set; }
    public int OptionIsDefault { get; set; }
    public bool OptionIsHidden { get; set; }
    public int Live { get; set; } = 1;
}
