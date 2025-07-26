using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProject.Api.Models;
using System.Collections.Generic;

namespace MiniProject.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        [HttpGet("chart-data")]
        public ActionResult<IEnumerable<ChartData>> GetChartData()
        {
            // Hardcoded sample data
            var data = new List<ChartData>
            {
                new ChartData { Label = "Open", Value = 10 },
                new ChartData { Label = "In Progress", Value = 5 },
                new ChartData { Label = "Completed", Value = 8 },
                new ChartData { Label = "Closed", Value = 15 }
            };

            return Ok(data);
        }
    }
}
