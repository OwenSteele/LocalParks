using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class ParkEventsController : Controller
    {
        private readonly ILogger<ParkEventsController> _logger;
        private readonly IParkEventsService _service;
        private readonly IAuthenticationService _authenticationService;
        private ParkEventModel _tempEvent;

        public ParkEventsController(ILogger<ParkEventsController> logger, IParkEventsService service,
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _service = service;
            _authenticationService = authenticationService;
        }

        [BindProperty]
        public ParkEventModel Event
        {
            get { return _tempEvent; }
            set { _tempEvent = value; }
        }

        public async Task<IActionResult> Index(
            string searchTerm = null,
            string parkFilter = null,
            DateTime? date = null,
            string sortBy = null)
        {
            _logger.LogInformation("Executing ParkEvents.Index Model");

            ViewData["Parks"] = await _service.GetParkSelectListItemsAsync(true);
            ViewData["SortOptions"] = _service.GetSortSelectListItems();

            if (await _authenticationService.IsSignedIn(this.User))
            {
                    ViewData["User"] = "User";
            }

            if (string.IsNullOrWhiteSpace(searchTerm) &&
                string.IsNullOrWhiteSpace(parkFilter) &&
                date == null)
            {
                var parkEvents = await _service.GetAllParkEventModelsAsync(sortBy: sortBy);
                return View(parkEvents);
            }

            var matches = await _service.GetSearchedParkEventModelsAsync(
                searchTerm, parkFilter, date, sortBy);

            if (matches != null)
            {
                TempData["Filter"] = searchTerm;

                if (!string.IsNullOrWhiteSpace(parkFilter)
                    || !string.IsNullOrWhiteSpace(sortBy)
                    || date != null)
                    TempData["FilteredSorted"] = "true";
            }
            else
            {
                TempData["Matches"] = "No Matches found";

                var parkEvents = await _service.GetAllParkEventModelsAsync(sortBy: sortBy);
                return View(parkEvents);
            }

            return View(matches);
        }

        public async Task<IActionResult> Details(int parkId, DateTime date)
        {
            _logger.LogInformation("Executing ParkEvents.Details Model");

            var parkEvent = await _service.GetParkEventModelAsync(parkId, date);

            if (await _authenticationService.IsSignedIn(this.User))
            {
                if (await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
                    ViewData["User"] = "Admin";

                else
                    ViewData["User"] = _service.GetEventOwner(parkId, date, this.User.Identity.Name);
            }

            if (parkEvent == null) return View("NotFound");

            return View(parkEvent);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int parkId, DateTime date)
        {
            _logger.LogInformation("Executing ParkEvents.Details Model");

            if (!await _authenticationService.IsSignedIn(this.User))
                return RedirectToAction("Details", "ParkEvents",
                    new { _tempEvent.ParkId, _tempEvent.Date });

            if (parkId != 0 &&
                date != DateTime.MinValue &&
                await _service.GetEventOwner(parkId, date, this.User.Identity.Name) == null &&
                !await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
                return RedirectToAction("Details", "ParkEvents", new { parkId, date });

            ViewData["Parks"] = await _service.GetParkSelectListItemsAsync();

            if (parkId == 0 && date == DateTime.MinValue)
                return View(new ParkEventModel());

            var result = await _service.GetParkEventModelAsync(parkId, date);

            if (result == null) return View("NotFound");

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit()
        {
            if (!await _authenticationService.IsSignedIn(this.User))
                return RedirectToAction("Details", "ParkEvents",
                    new { _tempEvent.ParkId, _tempEvent.Date });

            ViewData["Parks"] = await _service.GetParkSelectListItemsAsync();

            if (!ModelState.IsValid)
            {
                return View("Edit", _tempEvent);
            }

            var match = await _service.GetParkAsync(_tempEvent.ParkId);
            if (match == null)
            {
                return View("Edit", _tempEvent);
            }

            if (match.Events.FirstOrDefault(e => e.Date == _tempEvent.Date) != null)
            {
                return View("Edit", _tempEvent);
            }

            var result = await _service.AddNewParkEventAsync(_tempEvent, this.User.Identity.Name, false);

            if (result != null)
            {
                return RedirectToAction("Details", "ParkEvents", new { result.ParkId, result.Date });
            }

            return View("Edit", _tempEvent);
        }

        public async Task<IActionResult> Delete(int parkId, DateTime date, bool confirmed = false)
        {
            if (!await _authenticationService.IsSignedIn(this.User))
                return RedirectToAction("Details", "ParkEvents", new { parkId, date });

            if (await _service.GetEventOwner(parkId, date, this.User.Identity.Name) == null &&
                !await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
                return RedirectToAction("Details", "ParkEvents", new { parkId, date });

            var result = await _service.GetParkEventModelAsync(parkId, date);
            if (result == null) RedirectToAction("NotFound", "ParkEvents");

            if (!confirmed) return View(result);

            if (await _service.DeleteParkEventAsync(result))
                return RedirectToAction("Index", "ParkEvents");

            return RedirectToAction("Details", "ParkEvents", new { parkId, date });
        }
    }
}
