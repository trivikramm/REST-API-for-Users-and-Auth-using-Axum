using LoginDashboardAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoginDashboardAPI.Services
{
    public interface IDashboardService
    {
        Task<List<ChartData>> GetChartDataAsync();
    }
}