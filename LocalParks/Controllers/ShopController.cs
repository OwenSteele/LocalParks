using Microsoft.AspNetCore.Mvc;

namespace LocalParks.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
