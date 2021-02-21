using AutoMapper;
using LocalParks.Data;
using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class SupervisorsController : Controller
    {
        private readonly ILogger<SupervisorsController> _logger;
        private readonly SupervisorsService _service;

        public SupervisorsController(ILogger<SupervisorsController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _service = new SupervisorsService(parkRepository, mapper);
        }

        public async Task<IActionResult> Index(string searchTerm = null)
        {
            _logger.LogInformation("Executing Supervisors.Index Model");

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var supervisors = await _service.GetAllSupervisorModelsAsync();
                return View(supervisors);
            }

            var matches = await _service.GetSearchedSupervisorModelsAsync(searchTerm);

            if (matches != null) TempData["Filter"] = searchTerm;
            else TempData["Matches"] = "No Matches found";

            return View(matches);
        }
        public async Task<IActionResult> Details(int parkId)
        {
            _logger.LogInformation("Executing Supervisors.Details Model");

            var supervisor = await _service.GetSupervisorModelAsync(parkId);

            if (supervisor == null) return View("NotFound");

            return View(supervisor);
        }
    }
}
