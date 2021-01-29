using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class SupervisorsController : Controller
    {
        private readonly ILogger<SupervisorsController> _logger;
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public SupervisorsController(ILogger<SupervisorsController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchTerm = null)
        {
            _logger.LogInformation("Executing Supervisors.Index Model");

            var results = await _parkRepository.GetAllSupervisorsAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();


                var matches = results.Where(p =>
                string.Join(" ",p.FirstName, p.LastName).ToLower() == searchTerm |
                string.Join(" ", p.FirstName, p.LastName).ToLower().Contains(searchTerm) |
                string.Join(" ", p.FirstName, p.LastName).ToLower().StartsWith(searchTerm))
                    .ToArray();

                if (matches.Any())
                {
                    results = matches;
                    TempData["Filter"] = searchTerm;
                }

                else TempData["Matches"] = "No Matches found";
            }

            return View(_mapper.Map<SupervisorModel[]>(results));
        }
        public async Task<IActionResult> Details(int parkId)
        {
            _logger.LogInformation("Executing Supervisors.Details Model");

            var result = await _parkRepository.GetSupervisorByParkIdAsync(parkId);

            if (result == null)
                return View("NotFound");

            var model = _mapper.Map<SupervisorModel>(result);

            return View(model);
        }
    }
}
