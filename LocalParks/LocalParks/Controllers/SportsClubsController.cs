using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
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
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public SportsClubsController(ILogger<ParksController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            _logger.LogInformation("Executing SportsClubs.Index Model");

            var results = await _parkRepository.GetAllSportsClubsAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                var matches = results.Where(p =>
                p.Name.ToLower() == searchTerm |
                p.Name.ToLower().Contains(searchTerm) |
                p.Name.ToLower().StartsWith(searchTerm))
                    .ToArray();

                if (matches.Any())
                {
                    results = matches;
                    TempData["Filter"] = searchTerm;
                }

                else TempData["Matches"] = "No Matches found";
            }

            return View(_mapper.Map<SportsClubModel[]>(results));
        }
        public async Task<IActionResult> Details(int sportsClubId)
        {
            _logger.LogInformation("Executing SportsClubs.Details Model");

            var result = await _parkRepository.GetSportsClubByIdAsync(sportsClubId);

            if (result == null)
                return View("NotFound");

            var model = _mapper.Map<SportsClubModel>(result);

            return View(model);
        }
        public IActionResult PageNotFound()
        {
            _logger.LogInformation("Page request not found, redirected SportsClubs.NotFound");

            return View();
        }
    }
}
