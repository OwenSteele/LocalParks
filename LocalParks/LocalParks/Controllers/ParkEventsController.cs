using LocalParks.Models;
using LocalParks.Services;
using LocalParks.Services.View;
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
        private readonly ISelectListService _listService;
        private ParkEventModel _tempEvent;

        public ParkEventsController(ILogger<ParkEventsController> logger,
            IParkEventsService service,
            IAuthenticationService authenticationService,
            ISelectListService listService
            )
        {
            _logger = logger;
            _service = service;
            _authenticationService = authenticationService;
            _listService = listService;
        }

        [BindProperty]
        public ParkEventModel Event
        {
            get { return _tempEvent; }
            set { _tempEvent = value; }
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Executing ParkEvents.Index Model");

            await SetViewData();

            if (await _authenticationService.IsSignedInAsync(User))
            {
                ViewData["User"] = "User";
            }

            var results = await _service.GetAllParkEventModelsAsync();
            return View(results);
        }

        public async Task<IActionResult> Filter(
            [FromServices] ISortingService sortingService,
            string searchTerm = null,
            string parkFilter = null,
            DateTime? date = null,
            string sortBy = null)
        {
            _logger.LogInformation("Executing ParkEvents.Index Model");

            await SetViewData();

            if (await _authenticationService.IsSignedInAsync(User))
            {
                ViewData["User"] = "User";
            }

            var matches = await _service.GetSearchedParkEventModelsAsync(
                searchTerm, parkFilter, date);

            if (matches != null)
            {

                if (!string.IsNullOrWhiteSpace(searchTerm)) TempData["Filter"] = searchTerm;

                else if (!string.IsNullOrWhiteSpace(sortBy) ||
                    !string.IsNullOrWhiteSpace(parkFilter) ||
                    date.HasValue) TempData["Sorted"] = "true";
            }
            else
            {
                TempData["Matches"] = "No Matches found";

                matches = await _service.GetAllParkEventModelsAsync();
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
                matches = sortingService.SortResults<ParkEventModel>(matches, sortBy);

            return View("Index", matches);
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
                ModelState.AddModelError("", "Invalid fields - see fields for issues.");
                return View("Edit", _tempEvent);
            }

            var match = await _service.GetParkAsync(_tempEvent.ParkId);
            if (match == null)
            {
                ModelState.AddModelError("", "This park doesn't exist");
                return View("Edit", _tempEvent);
            }

            if (match.Events.FirstOrDefault(e => e.Date == _tempEvent.Date) != null)
            {
                ModelState.AddModelError("", $"{match.Name} already has an event scheduled on {_tempEvent.Date.ToShortDateString()} - please choose another date.");
                return View("Edit", _tempEvent);
            }

            var result = await _service.AddNewParkEventAsync(_tempEvent, User.Identity.Name, false);

            if (result != null)
            {
                return RedirectToAction("Details", "ParkEvents", new { result.EventId });
            }

            ModelState.AddModelError("", "Could not create event - please try again later, or contact us with an issue.");
            return View("Edit", _tempEvent);
        }

        [HttpPost]
        public async Task<IActionResult> EditExisting()
        {
            if (!await _authenticationService.IsSignedInAsync(User))
                return RedirectToAction("Details", "ParkEvents",
                    new { _tempEvent.ParkId, _tempEvent.Date });

            ViewData["Parks"] = await _service.GetParkSelectListItemsAsync();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid fields - see fields for issues.");
                return View("Edit", _tempEvent);
            }

            var existing = await _service.GetParkEventModelByIdAsync(_tempEvent.EventId);

            if (existing == null) return View("NotFound");

            var park = await _service.GetParkAsync(_tempEvent.ParkId);

            if (park == null)
            {
                ModelState.AddModelError("", "This park doesn't exist");
                return View("Edit", _tempEvent);
            }

            if (park.Events.FirstOrDefault(e => e.Date == _tempEvent.Date) != null)
            {
                ModelState.AddModelError("", $"{park.Name} already has an event scheduled on {_tempEvent.Date.ToShortDateString()} - please choose another date.");
                return View("Edit", _tempEvent);
            }

            existing = _tempEvent;

            var result = await _service.UpdateParkEventAsync(existing);

            if (result != null)
            {
                return RedirectToAction("Details", "ParkEvents", new { result.EventId });
            }

            ModelState.AddModelError("", "Could not update event - please try again later, or contact us with an issue.");
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

            if (!confirmed) return View(result);

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

        private async Task SetViewData()
        {
            ViewData["Parks"] = await _service.GetParkSelectListItemsAsync(true);
            ViewData["SortOptions"] = _listService.GetSortSelectListItems<ParkEventModel>();
        }
    }
}
