using MachForm.NetCore.Models.Elements;

namespace MachForm.NetCore.Models.Forms;

public class FormDto
{
    public FormStatus Status { get; internal set; }
    public string Name { get; internal set; }
    public List<Element> Elements { get; internal set; }
    public int Id { get; internal set; }
    public DateTime CreatedDate { get; internal set; }
    public string Tags { get; internal set; }
}