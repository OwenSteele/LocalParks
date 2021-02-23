using AutoMapper;
using LocalParks.Data;
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
        private readonly ParkEventsService _service;
        private ParkEventModel _tempEvent;

        public ParkEventsController(ILogger<ParkEventsController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _service = new ParkEventsService(parkRepository, mapper);
        }

        [BindProperty]
        public ParkEventModel Event
        {
            get { return _tempEvent; }
            set { _tempEvent = value; }
        }

        public async Task<IActionResult> Index(
            string searchTerm = null,
            string parkValue = null,
            DateTime? date = null,
            string sortBy = null)
        {
            _logger.LogInformation("Executing ParkEvents.Index Model");

            ViewData["Parks"] = await _service.GetParkSelectListItemsAsync(true);
            ViewData["SortOptions"] = _service.GetSortSelectListItems(typeof(ParkEventModel));

            if (string.IsNullOrWhiteSpace(searchTerm) &&
                string.IsNullOrWhiteSpace(parkValue) &&
                date == null)
            {
                var parkEvents = await _service.GetAllParkEventModelsAsync(sortBy: sortBy);
                return View(parkEvents);
            }

            var matches = await _service.GetSearchedParkEventModelsAsync(
                searchTerm, parkValue, date, sortBy);

            if (matches != null)
            {
                TempData["Filter"] = searchTerm;

                if (!string.IsNullOrWhiteSpace(parkValue)
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

            if (parkEvent == null) return View("NotFound");

            return View(parkEvent);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int parkId, DateTime date)
        {
            _logger.LogInformation("Executing ParkEvents.Details Model");

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
            if (!ModelState.IsValid)
            {
                ViewData["Parks"] = await _service.GetParkSelectListItemsAsync();

                return View("Edit", _tempEvent);
            }

            var match = await _service.GetParkAsync(_tempEvent.ParkId);
            if (match == null)
            {
                ViewData["Parks"] = await _service.GetParkSelectListItemsAsync();

                return View("Edit", _tempEvent);
            }

            if (match.Events.FirstOrDefault(e => e.Date == _tempEvent.Date) != null)
            {
                ViewData["DateTaken"] = $"An Event is already booked for this date at {match.Name}";

                ViewData["Parks"] = await _service.GetParkSelectListItemsAsync();

                return View("Edit", _tempEvent);
            }

            var result = await _service.CreateNewEventAsync(_tempEvent, match);

            if (result != null)
            {
                return RedirectToAction("Details", "ParkEvents", new { result.ParkId, result.Date });
            }

            ViewData["Parks"] = await _service.GetParkSelectListItemsAsync();

            return View("Edit", _tempEvent);
        }

        public async Task<IActionResult> Delete(int parkId, DateTime date, bool confirmed = false)
        {
            var result = await _service.GetParkEventModelAsync(parkId, date);
            if (result == null) RedirectToAction("NotFound", "ParkEvents");

            if (!confirmed) return View(result);

            if (await _service.RemoveEventAsync(result))
                return RedirectToAction("Index", "ParkEvents");

            return RedirectToAction("Details", "ParkEvents", new { parkId, date });
        }
    }
}
