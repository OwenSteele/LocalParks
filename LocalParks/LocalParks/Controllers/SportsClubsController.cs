using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    [BindProperties]
    public class SportsClubsController : Controller
    {
        private readonly ILogger<ParksController> _logger;
        private readonly SportsClubsService _service;

        public SportsClubsController(ILogger<ParksController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _service = new SportsClubsService(parkRepository, mapper);
        }

        public async Task<IActionResult> Index(string searchTerm = null)
        {
            _logger.LogInformation("Executing SportsClubs.Index Model");

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var sportsClubs = await _service.GetAllSportsClubModelsAsync();
                return View(sportsClubs);
            }

            var matches = await _service.GetSearchedSportsClubModelsAsync(searchTerm);

            if (matches == null) TempData["Filter"] = searchTerm;
            else TempData["Matches"] = "No Matches found";

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
