using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _service;

        public AccountController(ILogger<AccountController> logger, IAccountService service)
        {
            _logger = logger;
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            var model = await _service.GetUserAsync(this.User.Identity.Name);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            _logger.LogInformation("Executing UserAcount.Login Model");

            if (this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            _logger.LogInformation("Executing UserAcount.Login Post");

            if (ModelState.IsValid)
            {
                var user = await _service.SignInAttemptAsync(model);

                if (user == null) RedirectToAction("Index", "Home");

                if(Request.Query.Keys.Contains("ReturnUrl"))
                {
                    Redirect(Request.Query["ReturnUrl"].First());
                }

                RedirectToAction("Index");
            }
            
            ModelState.AddModelError("", "Failed to Login");

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _service.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
