using System.Diagnostics.CodeAnalysis;

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
}
