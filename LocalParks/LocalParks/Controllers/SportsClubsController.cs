using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    [BindProperties]
    public class SportsClubsController : Controller
    {
        private readonly ILogger<ParksController> _logger;
        private readonly ISportsClubsService _service;

        public SportsClubsController(ILogger<ParksController> logger, ISportsClubsService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index(
            string searchTerm = null,
            string parkFilter = null,
            string sportType = null,
            string sortBy = null)
        {
            _logger.LogInformation("Executing SportsClubs.Index Model");

            ViewData["Parks"] = await _service.GetParkSelectListItemsAsync(true);
            ViewData["Sports"] = _service.GetSportListItems();
            ViewData["SortOptions"] = _service.GetSortSelectListItems();

            if (string.IsNullOrWhiteSpace(searchTerm) &&
                string.IsNullOrWhiteSpace(parkFilter) &&
                string.IsNullOrWhiteSpace(sportType))
            {
                var sportsClubs = await _service.GetAllSportsClubModelsAsync(sortBy);
                return View(sportsClubs);
            }

            var matches = await _service.GetSearchedSportsClubModelsAsync(
                searchTerm, parkFilter, sportType, sortBy);

            if (matches != null)
            {
                TempData["Filter"] = searchTerm;

                if (!string.IsNullOrWhiteSpace(parkFilter)
                    || !string.IsNullOrWhiteSpace(sortBy)
                    || !string.IsNullOrWhiteSpace(sportType))
                    TempData["FilteredSorted"] = "true";
            }
            else
            {
                TempData["Matches"] = "No Matches found";

                var sportClubs = await _service.GetAllSportsClubModelsAsync(sortBy);
                return View(sportClubs);
            }

            return View(matches);
        }
        public async Task<IActionResult> Details(int sportsClubId)
        {
            _logger.LogInformation("Executing SportsClubs.Details Model");

            var sportsClub = await _service.GetSportsClubModelAsync(sportsClubId);

            if (sportsClub == null) return View("NotFound");

            return View(sportsClub);
        }
    }
}
