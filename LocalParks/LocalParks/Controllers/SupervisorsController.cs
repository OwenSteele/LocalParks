using LocalParks.Core.Contracts;
using LocalParks.Core.Models;
using LocalParks.Models;
using LocalParks.Services;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class SupervisorsController : Controller
    {
        private readonly ILogger<SupervisorsController> _logger;
        private readonly ISupervisorsService _service;

        public SupervisorsController(ILogger<SupervisorsController> logger,
            ISupervisorsService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Executing Supervisors.Index Model");

            var results = await _service.GetAllSupervisorModelsAsync();
            return View(results);
        }

        public async Task<IActionResult> Filter(
            [FromServices] ISortingService sortingService,
            string searchTerm = null,
            string parkFilter = null,
            string sortBy = null)
        {
            _logger.LogInformation("Executing Supervisors.Filter Model");

            var matches = await _service.GetSearchedSupervisorModelsAsync(
                searchTerm, parkFilter);

            if (matches != null)
            {
                if (!string.IsNullOrWhiteSpace(searchTerm)) TempData["Filter"] = searchTerm;

                else if (!string.IsNullOrWhiteSpace(sortBy) ||
                    !string.IsNullOrWhiteSpace(parkFilter)) TempData["Sorted"] = "true";
            }
            else
            {
                TempData["Matches"] = "No Matches found";

                matches = await _service.GetAllSupervisorModelsAsync();
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
                matches = sortingService.SortResults<SupervisorModel>(matches, sortBy);

            return View("Index", matches);
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
