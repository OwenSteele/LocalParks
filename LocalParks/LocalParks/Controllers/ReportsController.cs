using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
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

        public IActionResult Index()
        {
            _logger.LogInformation("Index page: getting report model.");

            return View();
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
