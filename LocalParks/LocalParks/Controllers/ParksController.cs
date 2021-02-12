using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    [BindProperties]
    public class ParksController : Controller
    {
        private readonly ILogger<ParksController> _logger;
        private readonly ParksService _service;

        public ParksController(ILogger<ParksController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _service = new ParksService(parkRepository, mapper);
        }

        public async Task<IActionResult> Index(string searchTerm = null)
        {
            _logger.LogInformation("Executing Parks.Index Model");

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var parks = await _service.GetAllParkModelsAsync();
                return View(parks);
            }

            var matches = await _service.GetSearchedParksAsync(searchTerm);

            if (matches == null) TempData["Filter"] = searchTerm;
            else TempData["Matches"] = "No Matches found";

            return View(matches);
        }

        public async Task<IActionResult> Details(int parkId)
        {
            _logger.LogInformation("Executing Parks.Details Model");

            var park = await _service.GetParkAsync(parkId);

            if (park == null) return View("NotFound");

            return View(park);
        }
    }
}
