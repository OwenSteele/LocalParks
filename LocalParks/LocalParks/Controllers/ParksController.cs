using AutoMapper;
using LocalParks.Data;
using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public async Task<IActionResult> Index(string searchTerm = null, string postcode = null, string sortBy = null)
        {
            _logger.LogInformation("Executing Parks.Index Model");

            ViewData["Postcodes"] = await _service.GetPostcodeSelectListItemsAsync();
            ViewData["SortOptions"] = _service.GetSortSelectListItems();

            if (string.IsNullOrWhiteSpace(searchTerm) && string.IsNullOrWhiteSpace(postcode))
            {
                var parks = await _service.GetAllParkModelsAsync(sortBy);
                return View(parks);
            }

            var matches = await _service.GetSearchedParksAsync(searchTerm, postcode, sortBy);

            if (matches != null)
            {
                if (!string.IsNullOrWhiteSpace(searchTerm)) TempData["FilterName"] = searchTerm;
                if (!string.IsNullOrWhiteSpace(postcode)) TempData["FilterPostcode"] = postcode;
            }
            else
            {
                TempData["Matches"] = "No Matches found";

                var parks = await _service.GetAllParkModelsAsync(sortBy);
                return View(parks);
            }

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
