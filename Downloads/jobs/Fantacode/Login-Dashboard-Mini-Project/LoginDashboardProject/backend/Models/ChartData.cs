namespace LoginDashboardAPI.Models
{
public class ChartData
{
    public string Label { get; set; } = string.Empty;
    public int Value { get; set; }
    
    // Additional properties to match DashboardService
    public string? Status { get; set; }
    public int Count { get; set; }
}
}