namespace MachForm.NetCore.Models.FormLocks;

public class FormLockDto
{
    public int FormId { get; set; }
    public string UserId { get; set; }
    public DateTime LockDate { get; set; }
}
