using LocalParks.Core.Contracts;
using LocalParks.Models.Accounts;
using LocalParks.Services.Account;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _service;
        private readonly IAccountDataService _dataservice;
        private readonly IUserService _userService;

        public AccountController(
            IAccountService service,
            IUserService userService,
            IAccountDataService dataservice)
        {
            _service = service;
            _dataservice = dataservice;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _userService.GetUserAsync(User.Identity.Name);

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!await _dataservice.SignInAttemptAsync(model))
            {
                ModelState.AddModelError("", "Username or Password Invalid.");

                return View(model);
            }

            return RedirectToAction(returnUrl ?? "Index");
        }
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _service.SignOutAsync();

            if (!string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);

            return RedirectToAction(returnUrl ?? "Index", "Home");
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            return View(new SignInViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignInViewModel model)
        {

            bool error = false;

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
        public async Task<IActionResult> Developers()
        {
            var user = await _userService.GetUserAsync(User.Identity.Name);

            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> TokenGenerator()
        {
            var user = await _userService.GetUserAsync(User.Identity.Name);

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> TokenGenerator(string requested,
            [FromServices] ITokenService tokenService)
        {
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
            return View(new ChangePasswordViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
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
            var user = await _dataservice.GetChangeDetailsModelAsync(User.Identity.Name);

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditDetails(ChangeDetailsViewModel model)
        {
            if (!await _dataservice.ChangeDetailsAsync(model, User.Identity.Name))
            {
                ModelState.AddModelError("", "Details could not be changed");

                return View(model);
            }

            TempData["Success"] = "Password changed successfully.";

            return View(new ChangeDetailsViewModel());
        }
        [HttpGet]
        public IActionResult DeleteUserAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUserAccount(string confirmed = null)
        {
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
            var result = await orderService.GetUserOrdersAsync(User.Identity.Name);

            return View(result);
        }
    }
}