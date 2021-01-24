﻿using AutoMapper;
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

        private EventModel _tempEvent;

        public ParkEventsController(ILogger<ParkEventsController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        [BindProperty]
        public EventModel Event { 
            get { return _tempEvent; } 
            set { _tempEvent = value; }
        }

        public async Task<IActionResult> Index(string searchTerm)
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

            return View(_mapper.Map<EventModel[]>(results));
        }

        public async Task<IActionResult> Details(int parkId, DateTime date)
        {
            _logger.LogInformation("Executing ParkEvents.Details Model");

            var result = await _parkRepository.GetEventByParkIdByDateAsync(parkId, date);

            if (result == null)
                return View("NotFound");

            var model = _mapper.Map<EventModel>(result);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int parkId, DateTime date)
        {
            _logger.LogInformation("Executing ParkEvents.Details Model");

            if (parkId == 0 && date == DateTime.MinValue)
            {
                var newEventModel = new EventModel
                {
                    ParksList = await GetParks()
                };

                return View(newEventModel);
            }

            var result = await _parkRepository.GetEventByParkIdByDateAsync(parkId, date);

            if (result == null)
                return View("NotFound");

            var model = _mapper.Map<EventModel>(result);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit()
        {
            if (!ModelState.IsValid)
            {
                _tempEvent.ParksList = await GetParks();
                return View("Edit", _tempEvent);
            }
            _tempEvent.ParksList = await GetParks();

            var match = await _parkRepository.GetParkByIdAsync(_tempEvent.Park.ParkId);

            if (match == null)
            {
                _tempEvent.ParksList = await GetParks();
                return View("Edit", _tempEvent);
            }

            if (match.Events.FirstOrDefault(e => e.Date == _tempEvent.Date) != null)
            {
                ViewData["DateTaken"] = $"An Event is already booked for this date at {match.Name}";
                _tempEvent.ParksList = await GetParks();
                return View("Edit", _tempEvent);
            }

            _tempEvent.Park = match;

            var newEvent = _mapper.Map<Event>(_tempEvent);

            _parkRepository.Add(newEvent);

            if (await _parkRepository.SaveChangesAsync()) 
            {
                return RedirectToAction("Details", "ParkEvents", new { newEvent.Park.ParkId, newEvent.Date });
            }

            _tempEvent.ParksList = await GetParks();
            return View("Edit", _tempEvent);
        }

        public async Task<IActionResult> Delete(int parkId, DateTime date, bool confirmed = false)
        {
            var existing = await _parkRepository.GetEventByParkIdByDateAsync(parkId, date);
            if (existing == null) RedirectToAction("NotFound", "ParkEvents");

            if (!confirmed) return View(_mapper.Map<EventModel>(existing));

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
