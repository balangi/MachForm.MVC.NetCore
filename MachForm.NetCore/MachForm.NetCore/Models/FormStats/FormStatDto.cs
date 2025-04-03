using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MachForm.NetCore.Models.Forms;

namespace MachForm.NetCore.Models.FormStats;

public class FormStatDto
{
    public int FormId { get; set; }
    [AllowNull]
    public int TotalEntries { get; set; } = default(int);
    [AllowNull]
    public int TodayEntries { get; set; } = default(int);
    [AllowNull]
    public DateTime LastEntryDate { get; set; } = default(DateTime);


    [InverseProperty(nameof(FormDto.FormStatInfo))]
    public ICollection<FormDto> Forms { get; set; }
}
