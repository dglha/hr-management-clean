namespace HR.LeaveManagement.Application.Contracts.Identity;

public class JwsSettings
{
    public string Key { get; set; }
    public string Issuser { get; set; }
    public string Audience { get; set; }
    public double DurationInMinutes { get; set; }
}