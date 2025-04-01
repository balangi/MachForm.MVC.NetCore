namespace MachForm.NetCore.Services.Auth;

public class UserTokenDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public TokenType TokenType { get; set; }
}
