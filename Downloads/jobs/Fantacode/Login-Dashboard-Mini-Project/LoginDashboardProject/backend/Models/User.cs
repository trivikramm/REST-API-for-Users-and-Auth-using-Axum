namespace LoginDashboardAPI.Models
{
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Password { get; set; } // For demo purposes
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
}