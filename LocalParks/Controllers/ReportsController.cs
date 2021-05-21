using LocalParks.Services.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class ReportsController : Controller
    {
        public ReportsController()
        {
        }

        public async Task<IActionResult> Index([FromServices] IReportsService service)
        {
            var model = await service.GetReportsModelAsync();

            return View(model);
        }
        public IActionResult Parks()
        {
            return View();
        }
        public IActionResult Supervisors()
        {
            return View();
        }
        public IActionResult SportsClubs()
        {
            return View();
        }
        public IActionResult ParkEvents()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }
    }
}
