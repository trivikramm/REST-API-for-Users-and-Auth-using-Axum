using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginDashboardAPI.Models;

namespace LoginDashboardAPI.Services
{
    public class DashboardService : IDashboardService
    {
        public Task<List<ChartData>> GetChartDataAsync()
        {
            var chartData = new List<ChartData>
            {
                new ChartData { Label = "Open", Value = 10, Status = "Open", Count = 10 },
                new ChartData { Label = "In Progress", Value = 5, Status = "In Progress", Count = 5 },
                new ChartData { Label = "Closed", Value = 15, Status = "Closed", Count = 15 }
            };

            return Task.FromResult(chartData);
        }
    }
}