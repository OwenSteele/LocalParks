using LocalParks.Models;
using LocalParks.Services;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _service;

        public HomeController(ILogger<HomeController> logger, IHomeService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index(string latitude = null, string longitude = null)
        {
            _logger.LogInformation("Index page: getting home model.");

            var result = await _service.GetHomeModelAsync(latitude, longitude);

            return View("Index", result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Contact(
            [FromServices] ISelectListService listService)
        {
            ViewData["Postcodes"] = await listService.GetPostcodeSelectListItemsAsync();

            return View(new ContactViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model,
            [FromServices] ISelectListService listService)
        {
            ViewData["Postcodes"] = await listService.GetPostcodeSelectListItemsAsync();

            if (!ModelState.IsValid) return View(model);

            if (!_service.PostFeedBackAsync(model)) return View(model);

            ModelState.Clear();

            ViewData["FeedBackSuccess"] = "Thank you for you feedback!";

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