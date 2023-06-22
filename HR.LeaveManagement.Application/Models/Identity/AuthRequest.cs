namespace HR.LeaveManagement.Application.Contracts.Identity;

public class AuthRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}