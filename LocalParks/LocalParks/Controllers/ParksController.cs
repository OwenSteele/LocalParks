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
        private readonly IParksService _service;

        public ParksController(ILogger<ParksController> logger, IParksService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Executing Parks.Index Model");

            await SetViewData();

            var results = await _service.GetAllModelsAsync();

            return View(results);
        }

        public async Task<IActionResult> Filter(
            string searchTerm = null, 
            string postcode = null, 
            string sortBy = null)
        {
            _logger.LogInformation("Executing Parks.Filter");

            await SetViewData();

            if (string.IsNullOrWhiteSpace(searchTerm) && string.IsNullOrWhiteSpace(postcode))
            {
                var parks = await _service.GetAllModelsAsync(sortBy);

                if (!string.IsNullOrWhiteSpace(sortBy)) TempData["SortedBy"] = sortBy;

                return View("Index", parks);
            }

            var matches = await _service.GetSearchedAsync(searchTerm, postcode, sortBy);

            if (matches != null)
            {
                TempData["FilterValue"] = searchTerm;

                if (!string.IsNullOrWhiteSpace(postcode) || !string.IsNullOrWhiteSpace(postcode))
                    TempData["FilterName"] = "true";
            }
            else
            {
                TempData["Matches"] = "No Matches found";

                var parks = await _service.GetAllModelsAsync(sortBy);
                return View("Index", parks);
            }

            return View("Index", matches);
        }

        public async Task<IActionResult> Details(int parkId)
        {
            _logger.LogInformation("Executing Parks.Details Model");

            var park = await _service.GetParkAsync(parkId);

            if (park == null) return View("NotFound");

            return View(park);
        }
        private async Task SetViewData()
        {
            ViewData["Postcodes"] = await _service.GetPostcodeSelectListItemsAsync();
            ViewData["SortOptions"] = _service.GetSortSelectListItems();
        }

    }
}
