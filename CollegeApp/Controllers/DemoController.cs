using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        //1. Strongly coupled
        //private readonly IMyLogger _myLogger;
        //public DemoController(IMyLogger myLogger)
        //{
        //    _myLogger = new LogToFile();
        //}

        //2. Loosely coupled
        //private readonly IMyLogger _myLogger;
        //public DemoController(IMyLogger myLogger)
        //{
        //    _myLogger = myLogger;
        //}

        private readonly ILogger<DemoController> _logger;
        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogTrace("Trace method started");
            _logger.LogDebug("Debug method started");
            _logger.LogInformation("Information method started");
            _logger.LogWarning("Warning method started");
            _logger.LogError("Error method started");
            _logger.LogCritical("Critical method started");
            return Ok();
        }
    }
}
