using LocalParks.Services.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(ILogger<ReportsController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index([FromServices] IReportsService service)
        {
            var model = await service.GetReportsModelAsync();

            _logger.LogInformation("Index page: getting report model.");

            return View(model);
        }
        public IActionResult Parks()
        {
            _logger.LogInformation("Parks page: getting report model.");

            return View();
        }
        public IActionResult Supervisors()
        {
            _logger.LogInformation("Supervisors page: getting report model.");

            return View();
        }
        public IActionResult SportsClubs()
        {
            _logger.LogInformation("SportsClubs page: getting report model.");

            return View();
        }
        public IActionResult ParkEvents()
        {
            _logger.LogInformation("ParkEvents page: getting report model.");

            return View();
        }
        public IActionResult Shop()
        {
            _logger.LogInformation("Shop page: getting report model.");

            return View();
        }
    }
}
