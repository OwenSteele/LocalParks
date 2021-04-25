using LocalParks.Core.Contracts;
using LocalParks.Models;
using LocalParks.Models.Accounts;
using LocalParks.Services;
using LocalParks.Services.Account;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _service;
        private readonly IAccountDataService _dataservice;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISelectListService _listService;
        private readonly IUserService _userService;

        public AccountController(ILogger<AccountController> logger,
            IAccountService service,
            IAuthenticationService authenticationService,
            ISelectListService listService,
            IUserService userService,
            IAccountDataService dataservice)
        {
            _logger = logger;
            _service = service;
            _dataservice = dataservice;
            _authenticationService = authenticationService;
            _listService = listService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            var model = await _userService.GetUserAsync(User.Identity.Name);

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            _logger.LogInformation("Executing Account.Login Model");

            ViewData["ReturnUrl"] = returnUrl;

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            _logger.LogInformation("Executing Account.Login Post");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Login");

                return View(model);
            }

            if (!await _dataservice.SignInAttemptAsync(model))
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
        public async Task<IActionResult> Logout(
            [FromServices] IGuestAccountService guestAccountService, 
            string returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var username = User.Identity.Name;

            await _service.SignOutAsync();

            if (User.IsInRole("guest")) await guestAccountService.RemoveGuestAsync(username);

            if (!string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            ViewData["Postcodes"] = await _listService.GetPostcodeSelectListItemsAsync();

            return View(new SignInViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignInViewModel model)
        {
            ViewData["Postcodes"] = await _listService.GetPostcodeSelectListItemsAsync();

            bool error = false;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Create Profile - Check your fields.");

                return View("SignUp");
            }

            if (await _userService.GetUserAsync(model.Username) != null)
            {
                ModelState.AddModelError("", "This username is not available.");

                error = true;
            }

            if (await _userService.GetUserByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("", "An account is already associated with this email.");

                error = true;
            }

            if (error) return View("SignUp", model);

            var user = await _dataservice.AddUserAsync(model);

            if (user == null)
            {
                ModelState.AddModelError("", "Could not create user at this time, please try again later.");

                return View("SignUp", model);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult GuestAccount(string returnUrl = null)
        {
            _logger.LogInformation("Executing Account.GuestAccount Model");

            if (User.Identity.IsAuthenticated) return RedirectToAction("Index");

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GuestAccount(
            [FromServices] IGuestAccountService guestAccountService)
        {
            _logger.LogInformation("Executing Account.GuestAccount Post");
            
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index");

            if (!await guestAccountService.SignInAsync())
            {
                ModelState.AddModelError("", "Could not create a guest at this time");

                RedirectToAction("Login");
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Developers()
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            var user = await _userService.GetUserAsync(User.Identity.Name);

            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> TokenGenerator()
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            var user = await _userService.GetUserAsync(User.Identity.Name);

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> TokenGenerator(string requested,
            [FromServices] ITokenService tokenService)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            var user = await _userService.GetUserAsync(User.Identity.Name);

            if (requested == "true")
            {
                var token = tokenService.CreateUserToken(user);

                TempData["Token"] = token.Token;
                TempData["Expiry"] = token.Expiry;
            }

            return View(user);
        }
        public async Task<IActionResult> ApiDocumentation()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(null);
            }

            var user = await _userService.GetUserAsync(User.Identity.Name);

            return View(user);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            return View(new ChangePasswordViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await _service.CheckPasswordAsync(User.Identity.Name, model.Old))
            {
                ModelState.AddModelError("", "Existing password does not match.");

                return View(model);
            }

            if (!model.New.Equals(model.NewConfirmed))
            {
                ModelState.AddModelError("", "New passwords do not match.");

                return View(model);
            }

            if (!await _service.ChangePasswordAsync(User.Identity.Name, model.New))
            {
                ModelState.AddModelError("", "Could not change password - check new password meet requirements.");

                return View(model);
            }

            TempData["Success"] = "Password changed successfully.";

            return View(new ChangePasswordViewModel());

        }
        [HttpGet]
        public async Task<IActionResult> EditDetails()
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }
            var user = await _dataservice.GetChangeDetailsModelAsync(User.Identity.Name);

            ViewData["Postcodes"] = await _listService.GetPostcodeSelectListItemsAsync();

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditDetails(ChangeDetailsViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Postcodes"] = await _listService.GetPostcodeSelectListItemsAsync();

                return View(model);
            }

            if (!await _dataservice.ChangeDetailsAsync(model, User.Identity.Name))
            {
                ModelState.AddModelError("", "Details could not be changed");

                ViewData["Postcodes"] = await _listService.GetPostcodeSelectListItemsAsync();

                return View(model);
            }

            TempData["Success"] = "Password changed successfully.";

            return View(new ChangeDetailsViewModel());
        }
        [HttpGet]
        public IActionResult DeleteUserAccount()
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUserAccount(string confirmed = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            if (await _authenticationService.HasRequiredRoleAsync(User.Identity.Name, "Administrator"))
            {
                ModelState.AddModelError("", "Cannot delete this account");

                return RedirectToAction("Index");
            }

            if (confirmed == "true")
            {
                if (!await _service.DeleteUserAsync(User.Identity.Name))
                    return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult WhySignUp()
        {
            return View();
        }
        public async Task<IActionResult> MyOrders([FromServices] IOrderService orderService)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Please log on to access this");

                return RedirectToAction("Login");
            }

            var result = await orderService.GetUserOrdersAsync(User.Identity.Name);

            return View(result);
        }
    }
}