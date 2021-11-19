using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pfm.Services;

namespace pfm.Controllers
{
    [ApiController]
    [Route("/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IPfmService _pfmService;
        public CategoriesController(ILogger<CategoriesController> logger, IPfmService pfmService){
            _logger = logger;
            _pfmService = pfmService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCategories([FromForm] IFormFile file){
            var pom = await _pfmService.ImportCategories(file);
            return Ok(pom);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery(Name ="parent-id")] string parentId){
            var returnList = await _pfmService.GetCategories(parentId);
            return Ok(returnList);
        }
    }
}