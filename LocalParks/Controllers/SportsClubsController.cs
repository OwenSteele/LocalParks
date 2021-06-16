using LocalParks.Core.Contracts;
using LocalParks.Core.Models;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    [BindProperties]
    public class SportsClubsController : Controller
    {
        private readonly ISportsClubsService _service;

        public SportsClubsController(ISportsClubsService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Sports"] = _service.GetSportListItems();

            var results = await _service.GetSearchedSportsClubModelsAsync();

            return View(results);
        }
        public async Task<IActionResult> Filter(
            [FromServices] ISortingService sortingService,
            string searchTerm = null,
            string parkFilter = null,
            string sportType = null,
            string sortBy = null)
        {
            ViewData["Sports"] = _service.GetSportListItems();

            var matches = await _service.GetSearchedSportsClubModelsAsync(
                searchTerm, parkFilter, sportType);

            if (matches != null)
            {
                TempData["Filter"] = searchTerm;

                if (!string.IsNullOrWhiteSpace(sortBy) ||
                    !string.IsNullOrWhiteSpace(parkFilter) ||
                    !string.IsNullOrWhiteSpace(sportType)) TempData["Sorted"] = "true";
            }
            else
            {
                TempData["Matches"] = "No Matches found";

                matches = await _service.GetAllSportsClubModelsAsync();

            }

            if (!string.IsNullOrWhiteSpace(sortBy))
                matches = sortingService.SortResults(matches, sortBy);

            return View("Index", matches);

        }
        public async Task<IActionResult> Details(int sportsClubId)
        {
            var sportsClub = await _service.GetSportsClubModelAsync(sportsClubId);

            if (sportsClub == null) return View("NotFound");

            return View(sportsClub);
        }
    }
}
