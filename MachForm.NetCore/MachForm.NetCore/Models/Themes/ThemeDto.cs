namespace MachForm.NetCore.Models.Themes;

public class ThemeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsBuiltIn { get; set; }
    public ThemeStatus Status { get; internal set; }
}

