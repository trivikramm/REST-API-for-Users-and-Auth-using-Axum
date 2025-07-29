using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoginDashboardAPI.Models;
using LoginDashboardAPI.Services;

namespace LoginDashboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("chart-data")]
        public async Task<ActionResult<List<ChartData>>> GetChartData()
        {
            var chartData = await _dashboardService.GetChartDataAsync();
            return Ok(chartData);
        }
    }
}