using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomeService _service;

        public HomeController(ILogger<HomeController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _service = new HomeService(parkRepository, mapper);
        }

        public async Task<IActionResult> Index(string latitude = null, string longitude = null)
        {
            _logger.LogInformation("Index page: getting all parks.");

            var result = await _service.GetHomeModelAsync(latitude, longitude);

            return View("Index", result);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult AboutMe()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
