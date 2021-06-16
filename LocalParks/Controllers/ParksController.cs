using LocalParks.Core.Contracts;
using LocalParks.Core.Models;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    [BindProperties]
    public class ParksController : Controller
    {
        private readonly IParksService _service;

        public ParksController(IParksService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var results = await _service.GetAllModelsAsync();

            return View(results);
        }

        public async Task<IActionResult> Filter(
            [FromServices] ISortingService sortingService,
            string searchTerm = null,
            string postcode = null,
            string sortBy = null,
            string openOnly = null)
        {
            var openFilter = !string.IsNullOrEmpty(openOnly);
            var matches = await _service.GetSearchedAsync(searchTerm, postcode, openFilter);

            if (matches != null)
            {
                TempData["Filter"] = searchTerm;

                if (!string.IsNullOrWhiteSpace(sortBy) ||
                    !string.IsNullOrWhiteSpace(postcode) ||
                    openFilter) TempData["Sorted"] = "true";
            }
            else
            {
                TempData["Matches"] = "No Matches found";

                matches = await _service.GetAllModelsAsync();
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
                matches = sortingService.SortResults<ParkModel>(matches, sortBy);

            return View("Index", matches);
        }

        public async Task<IActionResult> Details(int parkId)
        {
            var park = await _service.GetParkAsync(parkId);

            return View(park);
        }

    }
}
