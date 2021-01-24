using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index page: getting all parks.");

            var results = await _parkRepository.GetAllParksAsync();

            return View(_mapper.Map<ParkModel[]>(results));
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
