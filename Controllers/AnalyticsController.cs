using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pfm.Models;
using pfm.Services;

namespace pfm.Controllers
{
    [ApiController]
    [Route("/spending-analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ILogger<AnalyticsController> _logger;
        private readonly IPfmService _pfmService;
        public AnalyticsController(ILogger<AnalyticsController> logger, IPfmService pfmService){
            _logger = logger;
            _pfmService = pfmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpendingAnalytics([FromQuery] string catcode, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] Directions? direction){
            var spendingAnalytics = await _pfmService.GetSpendingAnalytics(catcode, startDate, endDate, direction);
            return Ok(spendingAnalytics);
        }
    }
}