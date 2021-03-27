using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.API
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _service;
        private readonly ITokenService _tokenService;

        public AccountController(ILogger<AccountController> logger,
            IUserService service,
            ITokenService tokenService)
        {
            _logger = logger;
            _service = service;
            _tokenService = tokenService;
        }
        [HttpPost]
        [Route("api/[controller]/CreateToken")]
        public async Task<IActionResult> CreateToken(
            [FromBody] LoginModel model,
            [FromServices] IAccountService accountService)
        {
            _logger.LogInformation("CreateToken Request ApiAccountController");

            if (!ModelState.IsValid) return BadRequest();

            if (!await accountService.SignInAttemptAsync(model)) return BadRequest();

            var user = await _service.GetUserAsync(model.Username);

            var result = _tokenService.CreateUserToken(user);

            var token = new
            {
                token = result.Token,
                expiry = result.Expiry
            };

            return Created("", token);
        }
        [HttpGet]
        [Route("api/[controller]/GetShopToken")]
        public async Task<IActionResult> GetShopToken(
            [FromServices] IAuthenticationService authenticationService)
        {
            _logger.LogInformation("GetShopToken Request ApiAccountController");

            if (User == null ||
                User.Identity.Name == null ||
                !await authenticationService.IsSignedInAsync(User))
                return Unauthorized();

            var user = await _service.GetUserAsync(User.Identity.Name);

            if (user == null) return BadRequest();

            var result = _tokenService.CreateUserToken(user);

            var token = new
            {
                token = result.Token,
                expiry = result.Expiry
            };

            return Created("", token);
        }
    }
}
