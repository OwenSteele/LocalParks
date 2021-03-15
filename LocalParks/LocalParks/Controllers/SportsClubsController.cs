﻿using LocalParks.Services;
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

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Executing SportsClubs.Index Model");

            await SetViewData();

            var results = await _service.GetSearchedSportsClubModelsAsync();

            return View(results);
        }
        public async Task<IActionResult> Filter(
            string searchTerm = null,
            string parkFilter = null,
            string sportType = null,
            string sortBy = null)
        {
            _logger.LogInformation("Executing SportsClubs.Filter");

            await SetViewData();

            if (string.IsNullOrWhiteSpace(searchTerm) &&
                string.IsNullOrWhiteSpace(parkFilter) &&
                string.IsNullOrWhiteSpace(sportType))
            {
                var sportsClubs = await _service.GetAllSportsClubModelsAsync(sortBy);

                return View("Index", sportsClubs);
            }

            var matches = await _service.GetSearchedSportsClubModelsAsync(
                searchTerm, parkFilter, sportType, sortBy);

            if (matches == null)
            {
                TempData["Matches"] = "No Matches found";

                var sportClubs = await _service.GetAllSportsClubModelsAsync(sortBy);
                return View("Index", sportClubs);                
            }

            TempData["Filter"] = searchTerm;

            TempData["FilteredSorted"] = "true";

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
            ViewData["SortOptions"] = _service.GetSortSelectListItems();
        }
    }
}
