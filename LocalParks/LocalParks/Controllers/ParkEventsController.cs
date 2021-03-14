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

            if (await _authenticationService.IsSignedInAsync(User))
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

        public async Task<IActionResult> Details(int eventId)
        {
            _logger.LogInformation("Executing ParkEvents.Details Model");

            var parkEvent = await _service.GetParkEventModelByIdAsync(eventId);

            if (await _authenticationService.IsSignedInAsync(User))
            {
                if (await _authenticationService.HasRequiredRoleAsync(User.Identity.Name, "Administrator"))
                    ViewData["User"] = "Admin";

                else
                    ViewData["User"] = _service.GetEventOwner(eventId, User.Identity.Name);
            }

            if (parkEvent == null) return View("NotFound");

            return View(parkEvent);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int eventId)
        {
            _logger.LogInformation("Executing ParkEvents.Details Model");

            if (!await _authenticationService.IsSignedInAsync(User))
                return RedirectToAction("Index", "ParkEvents");

            if (eventId != 0 &&
                await _service.GetEventOwner(eventId, User.Identity.Name) == null &&
                !await _authenticationService.HasRequiredRoleAsync(User.Identity.Name, "Administrator"))
                return RedirectToAction("Details", "ParkEvents", new { eventId });

            ViewData["Parks"] = await _service.GetParkSelectListItemsAsync();

            if (eventId == 0)
                return View(new ParkEventModel());

            var result = await _service.GetParkEventModelByIdAsync(eventId);

            if (result == null) return View("NotFound");

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit()
        {
            if (!await _authenticationService.IsSignedInAsync(User))
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

            var result = await _service.AddNewParkEventAsync(_tempEvent, User.Identity.Name, false);

            if (result != null)
            {
                return RedirectToAction("Details", "ParkEvents", new { result.EventId });
            }

            return View("Edit", _tempEvent);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int eventId)
        {
            if (!await _authenticationService.IsSignedInAsync(User))
                return RedirectToAction("Details", "ParkEvents", new { eventId });

            if (await _service.GetEventOwner(eventId, User.Identity.Name) == null &&
                !await _authenticationService.HasRequiredRoleAsync(User.Identity.Name, "Administrator"))
                return RedirectToAction("Details", "ParkEvents", new { eventId });

            var result = await _service.GetParkEventModelByIdAsync(eventId);
            if (result == null) RedirectToAction("NotFound", "ParkEvents");

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int eventId, bool confirmed)
        {
            if (!await _authenticationService.IsSignedInAsync(User))
                return RedirectToAction("Details", "ParkEvents", new { eventId });

            if (await _service.GetEventOwner(eventId, User.Identity.Name) == null &&
                !await _authenticationService.HasRequiredRoleAsync(User.Identity.Name, "Administrator"))
                return RedirectToAction("Details", "ParkEvents", new { eventId });

            var result = await _service.GetParkEventModelByIdAsync(eventId);
            if (result == null) RedirectToAction("NotFound", "ParkEvents");

            if(!confirmed) return View(result);

            try
            {
                if (await _service.DeleteParkEventAsync(result))
                    return RedirectToAction("Index", "ParkEvents");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Could not delete event. Please contact us about this issue");
                return View(result);
            }

            return RedirectToAction("Details", "ParkEvents", new { eventId });
        }

    }
}
