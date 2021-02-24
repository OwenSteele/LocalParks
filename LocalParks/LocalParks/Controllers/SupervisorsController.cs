using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class SupervisorsController : Controller
    {
        private readonly ILogger<SupervisorsController> _logger;
        private readonly ISupervisorsService _service;

        public SupervisorsController(ILogger<SupervisorsController> logger, ISupervisorsService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index(
            string searchTerm = null,
            string parkFilter = null,
            string sortBy = null)
        {
            _logger.LogInformation("Executing Supervisors.Index Model");

            ViewData["Parks"] = await _service.GetParkSelectListItemsAsync(true);
            ViewData["SortOptions"] = _service.GetSortSelectListItems();

            if (string.IsNullOrWhiteSpace(searchTerm) &&
                string.IsNullOrWhiteSpace(parkFilter))
            {
                var supervisors = await _service.GetAllSupervisorModelsAsync(sortBy);
                return View(supervisors);
            }

            var matches = await _service.GetSearchedSupervisorModelsAsync(
                searchTerm, parkFilter, sortBy);

            if (matches != null)
            {
                TempData["Filter"] = searchTerm;

                if (!string.IsNullOrWhiteSpace(parkFilter)
                    || !string.IsNullOrWhiteSpace(sortBy))
                    TempData["FilteredSorted"] = "true";
            }
            else
            {
                TempData["Matches"] = "No Matches found";

                var sportClubs = await _service.GetAllSupervisorModelsAsync(sortBy);
                return View(sportClubs);
            }

            return View(matches);
        }
        public async Task<IActionResult> Details(int parkId)
        {
            _logger.LogInformation("Executing Supervisors.Details Model");

            var supervisor = await _service.GetSupervisorModelAsync(parkId);

            if (supervisor == null) return View("NotFound");

            return View(supervisor);
        }
    }
}
