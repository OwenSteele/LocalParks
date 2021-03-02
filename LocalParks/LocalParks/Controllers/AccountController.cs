using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Login(string returnUrl = null)
        {
            _logger.LogInformation("Executing Acount.Login Model");

            ViewData["ReturnUrl"] = returnUrl;

            if (this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            return View(new LoginModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            _logger.LogInformation("Executing Acount.Login Post");

            if (ModelState.IsValid)
            {
                var user = await _service.SignInAttemptAsync(model);

                if (user == null) return RedirectToAction("Index", "Home");

                if(returnUrl != null)
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index");
            }
            
            ModelState.AddModelError("", "Failed to Login");

            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            await _service.SignOutAsync();

            if(!string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            if (this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            ViewData["Postcodes"] = await _service.GetPostcodeSelectListItemsAsync();

            return View(new SignInModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignInModel model)
        {
            ViewData["Postcodes"] = await _service.GetPostcodeSelectListItemsAsync();

            bool error = false;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Create Profile - Check your fields.");

                return View("SignUp", model);
            }

            if(await _service.GetUserAsync(model.Username) != null)
            {
                ModelState.AddModelError("", "This username is not available.");

                error = true;
            }

            if (await _service.GetUserByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("", "An account is already associated with this email.");

                error = true;
            }

            if(error) return View("SignUp",model);

            var user = await _service.AddUserAsync(model);

            if (user == null)
            {
                ModelState.AddModelError("", "Could not create user at this time, please try again later.");

                return View("SignUp", model);
            }

            return RedirectToAction("Index");
        }
        public IActionResult Developers()
        {
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        public IActionResult TokenGenerator()
        {
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        public IActionResult ApiDocumentation()
        {
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        public IActionResult ChangePassword()
        {
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        public IActionResult EditDetails()
        {
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpGet]
        public IActionResult DeleteUserAccount()
        {
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult >DeleteUserAccount(string confirmed = null)
        {
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if(confirmed == "true")
            {
                if (!await _service.DeleteUserAsync(this.User.Identity.Name))
                    return RedirectToAction("Index");
            }

            return RedirectToAction("Index","Home");
        }
        public IActionResult WhySignUp()
        {
            return View();
        }
    }
}