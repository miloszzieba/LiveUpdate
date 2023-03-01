using LiveUpdatePerformance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LiveUpdatePerformance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataService _dataService;

        public ValuesController(DataService dataService)
        {
            this._dataService = dataService;
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            return Ok(_dataService.GetCurrentRows());
        }
    }
}
