using LocalParks.Models;
using LocalParks.Services;
using LocalParks.Services.View;
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

        public SportsClubsController(ILogger<ParksController> logger,
            ISportsClubsService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Executing SportsClubs.Index Model");

            await SetViewData();

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
            _logger.LogInformation("Executing SportsClubs.Filter");

            await SetViewData();

            var matches = await _service.GetSearchedSportsClubModelsAsync(
                searchTerm, parkFilter, sportType);

            if (matches != null)
            {
                if (!string.IsNullOrWhiteSpace(searchTerm)) TempData["Filter"] = searchTerm;

                else if (!string.IsNullOrWhiteSpace(sortBy) ||
                    !string.IsNullOrWhiteSpace(parkFilter) ||
                    !string.IsNullOrWhiteSpace(sportType)) TempData["Sorted"] = "true";
            }
            else
            {
                TempData["Matches"] = "No Matches found";

                matches = await _service.GetAllSportsClubModelsAsync();

            }

            if (!string.IsNullOrWhiteSpace(sortBy))
                matches = sortingService.SortResults<SportsClubModel>(matches, sortBy);

            return View("Index", matches);

        }
        public async Task<IActionResult> Details(int sportsClubId)
        {
            _logger.LogInformation("Executing SportsClubs.Details Model");

            var sportsClub = await _service.GetSportsClubModelAsync(sportsClubId);

            if (sportsClub == null) return View("NotFound");

            return View(sportsClub);
        }

        private async Task SetViewData()
        {
            ViewData["Parks"] = await _service.GetParkSelectListItemsAsync(true);
            ViewData["Sports"] = _service.GetSportListItems();
        }
    }
}
