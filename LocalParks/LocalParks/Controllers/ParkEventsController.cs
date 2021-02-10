using AutoMapper;
using LocalParks.Data;
using LocalParks.Core;
using LocalParks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class ParkEventsController : Controller
    {
        private readonly ILogger<ParkEventsController> _logger;
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        private ParkEventModel _tempEvent;
        
        // This IOC is good, if you can demonstrate an awareness of the SOLID princeples you will be in a good place
        // I think the most important asepect is the S.. which is single responability which is .. each class having a single purpose
        // so as this is the controller layer having linq statements to filter is a violation.
        // And if you move it into a class for example PartEventsService.cs which enables you to write a unit test it if required and also
        // stops this class becoming unmaintable and very large
        public ParkEventsController(ILogger<ParkEventsController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        [BindProperty]
        public ParkEventModel Event { 
            get { return _tempEvent; } 
            set { _tempEvent = value; }
        }

        public async Task<IActionResult> Index(string searchTerm = null)
        {
            _logger.LogInformation("Executing ParkEvents.Index Model");

            var results = await _parkRepository.GetAllEventsAsync();

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

            return View(_mapper.Map<ParkEventModel[]>(results));
        }

        public async Task<IActionResult> Details(int parkId, DateTime date)
        {
            _logger.LogInformation("Executing ParkEvents.Details Model");

            var result = await _parkRepository.GetEventByParkIdByDateAsync(parkId, date);

            if (result == null)
                return View("NotFound");

            var model = _mapper.Map<ParkEventModel>(result);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int parkId, DateTime date)
        {
            _logger.LogInformation("Executing ParkEvents.Details Model");
            
            ViewData["Parks"] = await GetParks();

            if (parkId == 0 && date == DateTime.MinValue)
            {
                var newEventModel = new ParkEventModel();
                               
                return View(newEventModel);
            }

            var result = await _parkRepository.GetEventByParkIdByDateAsync(parkId, date);

            if (result == null)
                return View("NotFound");

            var model = _mapper.Map<ParkEventModel>(result);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit()
        {
            if (!ModelState.IsValid)
            {
                ViewData["Parks"] = await GetParks();

                return View("Edit", _tempEvent);
            }

            var match = await _parkRepository.GetParkByIdAsync(_tempEvent.ParkId);

            if (match == null)
            {
                ViewData["Parks"] = await GetParks();

                return View("Edit", _tempEvent);
            }

            if (match.Events.FirstOrDefault(e => e.Date == _tempEvent.Date) != null)
            {
                ViewData["DateTaken"] = $"An Event is already booked for this date at {match.Name}";

                ViewData["Parks"] = await GetParks();

                return View("Edit", _tempEvent);
            }

            var newEvent = _mapper.Map<ParkEvent>(_tempEvent);

            newEvent.Park = match;

            _parkRepository.Add(newEvent);

            if (await _parkRepository.SaveChangesAsync()) 
            {
                return RedirectToAction("Details", "ParkEvents", new { newEvent.Park.ParkId, newEvent.Date });
            }

            ViewData["Parks"] = await GetParks();

            return View("Edit", _tempEvent);
        }

        public async Task<IActionResult> Delete(int parkId, DateTime date, bool confirmed = false)
        {
            var existing = await _parkRepository.GetEventByParkIdByDateAsync(parkId, date);
            if (existing == null) RedirectToAction("NotFound", "ParkEvents");

            if (!confirmed) return View(_mapper.Map<ParkEventModel>(existing));

            _parkRepository.Delete(existing);

            if (await _parkRepository.SaveChangesAsync())
            {
                return RedirectToAction("Index", "ParkEvents");
            }
            return RedirectToAction("Details", "ParkEvents", new { parkId, date });
        }

        private async Task<IEnumerable<SelectListItem>> GetParks()
        {
            var parks = _mapper.Map<IEnumerable<ParkModel>>(await _parkRepository.GetAllParksAsync());

            return from p in parks
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = p.Name,
                       Value = p.ParkId.ToString()
                   };
        }
    }
}
