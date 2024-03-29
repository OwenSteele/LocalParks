﻿using LocalParks.Core.Contracts;
using LocalParks.Core.Contracts.Managers;
using LocalParks.Core.Models;
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
        private readonly IParkEventsService _service;
        private readonly IAuthenticationService _authenticationService;
        private ParkEventModel _tempEvent;

        public ParkEventsController(IParkEventsService service,
            IAuthenticationService authenticationService)
        {
            _service = service;
            _authenticationService = authenticationService;
        }

        [BindProperty]
        public ParkEventModel Event
        {
            get { return _tempEvent; }
            set { _tempEvent = value; }
        }

        public async Task<IActionResult> Index()
        {
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
                matches = sortingService.SortResults(matches, sortBy);

            return View("Index", matches);
        }

        public async Task<IActionResult> Details([FromServices] IParkEventsManager parkEventsManager, int eventId
            )
        {
            var parkEvent = await _service.GetParkEventModelByIdAsync(eventId);

            ViewData["User"] = await parkEventsManager.IsUserAuthorizedAsync(User?.Identity.Name, parkEvent);

            return View(parkEvent);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int eventId)
        {
            if (!await _authenticationService.IsSignedInAsync(User))
                return RedirectToAction("Index", "ParkEvents");

            if (eventId != 0 && await HasEditAccessAsync(eventId))
                return RedirectToAction("Details", "ParkEvents", new { eventId });
            else if (eventId == 0)
                return View(_service.GetNewEvent());

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

            if (match.Events.FirstOrDefault(e => e.Date == _tempEvent.Date && e.EventId != _tempEvent.EventId) != null)
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

            if (park.Events.FirstOrDefault(e => e.Date == _tempEvent.Date && e.EventId != _tempEvent.EventId) != null)
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

            if (!await HasEditAccessAsync(eventId))
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

            if (await HasEditAccessAsync(eventId))
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

        private async Task<bool> HasEditAccessAsync(int eventId)
        {
            return User.Identity != null &&
            await _service.GetEventOwner(eventId, User.Identity.Name) == null &&
            !await _authenticationService.HasRequiredRoleAsync(User.Identity.Name, "Administrator");
        }
    }
}
