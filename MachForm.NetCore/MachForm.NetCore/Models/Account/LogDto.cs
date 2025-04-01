namespace MachForm.NetCore.Models.Account;

public class LogDto
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string Level { get; set; }
    public DateTime Timestamp { get; set; }
}