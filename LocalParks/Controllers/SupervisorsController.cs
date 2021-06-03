using LocalParks.Core.Contracts;
using LocalParks.Core.Models;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class SupervisorsController : Controller
    {
        private readonly ISupervisorsService _service;

        public SupervisorsController(ISupervisorsService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var results = await _service.GetAllSupervisorModelsAsync();
            return View(results);
        }

        public async Task<IActionResult> Filter(
            [FromServices] ISortingService sortingService,
            string searchTerm = null,
            string parkFilter = null,
            string sortBy = null)
        {
            var matches = await _service.GetSearchedSupervisorModelsAsync(
                searchTerm, parkFilter);

            if (matches != null)
            {
                TempData["Filter"] = searchTerm;

                if (!string.IsNullOrWhiteSpace(sortBy) ||
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
            var supervisor = await _service.GetSupervisorModelAsync(parkId);

            if (supervisor == null) return View("NotFound");

            return View(supervisor);
        }
    }
}
