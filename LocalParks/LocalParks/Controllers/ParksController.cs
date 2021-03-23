using LocalParks.Models;
using LocalParks.Services;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    [BindProperties]
    public class ParksController : Controller
    {
        private readonly ILogger<ParksController> _logger;
        private readonly IParksService _service;
        private readonly ISelectListService _listService;

        public ParksController(ILogger<ParksController> logger,
            IParksService service,
            ISelectListService listService)
        {
            _logger = logger;
            _service = service;
            _listService = listService;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Executing Parks.Index Model");

            await SetViewData();

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
            _logger.LogInformation("Executing Parks.Filter");

            await SetViewData();

            var openFilter = !string.IsNullOrEmpty(openOnly);

            var matches = await _service.GetSearchedAsync(searchTerm, postcode, openFilter);

            if (matches != null)
            {
                if (!string.IsNullOrWhiteSpace(searchTerm)) TempData["Filter"] = searchTerm;

                else if (!string.IsNullOrWhiteSpace(sortBy) ||
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
            _logger.LogInformation("Executing Parks.Details Model");

            var park = await _service.GetParkAsync(parkId);

            if (park == null) return View("NotFound");

            return View(park);
        }
        private async Task SetViewData()
        {
            ViewData["Postcodes"] = await _service.GetPostcodeSelectListItemsAsync();
            ViewData["SortOptions"] = _listService.GetSortSelectListItems<ParkModel>();
        }

    }
}
