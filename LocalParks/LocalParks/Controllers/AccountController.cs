using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _service;
        private readonly IAuthenticationService _authenticationService;

        public AccountController(ILogger<AccountController> logger, IAccountService service,
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _service = service;
            _authenticationService = authenticationService;
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

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Login");

                return View(model);
            }

            var user = await _service.SignInAttemptAsync(model);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password Invalid.");

                return View(model);
            }

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            await _service.SignOutAsync();

            if (!string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);

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

                return View("SignUp");
            }

            if (await _service.GetUserAsync(model.Username) != null)
            {
                ModelState.AddModelError("", "This username is not available.");

                error = true;
            }

            if (await _service.GetUserByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("", "An account is already associated with this email.");

                error = true;
            }

            if (error) return View("SignUp", model);

            var user = await _service.AddUserAsync(model);

            if (user == null)
            {
                ModelState.AddModelError("", "Could not create user at this time, please try again later.");

                return View("SignUp", model);
            }

            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Developers()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            var user = await _service.GetUserAsync(this.User.Identity.Name);

            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> TokenGenerator()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            var user = await _service.GetUserAsync(this.User.Identity.Name);

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> TokenGenerator(string requested)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            var user = await _service.GetUserAsync(this.User.Identity.Name);

            if (requested == "true")
            {
                var token = await _service.GetUserTokenAsync(user);

                TempData["Token"] = token[0];
                TempData["Expiry"] = token[1];
            }

            return View(user);
        }
        public async Task<IActionResult> ApiDocumentation()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            var user = await _service.GetUserAsync(this.User.Identity.Name);

            return View(user);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            return View(new ChangePasswordModel());
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await _service.CheckPasswordAsync(this.User.Identity.Name, model.Old))
            {
                ModelState.AddModelError("", "Existing password does not match.");

                return View(model);
            }

            if (!model.New.Equals(model.NewConfirmed))
            {
                ModelState.AddModelError("", "New passwords do not match.");

                return View(model);
            }

            if (!await _service.ChangePasswordAsync(this.User.Identity.Name, model.New))
            {
                ModelState.AddModelError("", "Could not change password - check new password meet requirements.");

                return View(model);
            }

            TempData["Success"] = "Password changed successfully.";

            return View(new ChangePasswordModel());

        }
        [HttpGet]
        public async Task<IActionResult> EditDetails()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }
            var user = await _service.GetChangeDetailsModelAsync(this.User.Identity.Name);

            ViewData["Postcodes"] = await _service.GetPostcodeSelectListItemsAsync();

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditDetails(ChangeDetailsModel model)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Postcodes"] = await _service.GetPostcodeSelectListItemsAsync();

                return View(model);
            }

            if (!await _service.ChangeDetailsAsync(model, this.User.Identity.Name))
            {
                ModelState.AddModelError("", "Details could not be changed");

                ViewData["Postcodes"] = await _service.GetPostcodeSelectListItemsAsync();

                return View(model);
            }

            TempData["Success"] = "Password changed successfully.";

            return View(new ChangeDetailsModel());
        }
        [HttpGet]
        public IActionResult DeleteUserAccount()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUserAccount(string confirmed = null)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            if (await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
            {
                ModelState.AddModelError("", "Cannot delete this account");

                return RedirectToAction("Index");
            }

            if (confirmed == "true")
            {
                if (!await _service.DeleteUserAsync(this.User.Identity.Name))
                    return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult WhySignUp()
        {
            return View();
        }
    }
}