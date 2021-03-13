using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.API
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _service;
        private readonly IAuthenticationService _authenticationService;

        public AccountController(ILogger<AccountController> logger,
            IAccountService service, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _service = service;
            _authenticationService = authenticationService;
        }
        [HttpPost]
        [Route("api/[controller]/CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel model)
        {
            _logger.LogInformation("CreateToken Request ApiAccountController");

            if (!ModelState.IsValid) return BadRequest();

            var user = await _service.SignInAttemptAsync(model);

            if (user == null) return BadRequest();

            var token = await CreateTokenObjectAsync(user);

            return Created("", token);
        }
        [HttpGet]
        [Route("api/[controller]/GetShopToken")]
        public async Task<IActionResult> GetShopToken()
        {
            _logger.LogInformation("GetShopToken Request ApiAccountController");

            if (User == null ||
                User.Identity.Name == null ||
                !await _authenticationService.IsSignedIn(User))
                return Unauthorized();

            var user = await _service.GetUserAsync(User.Identity.Name);

            if (user == null) return BadRequest();

            var token = await CreateTokenObjectAsync(user);

            return Created("", token);
        }

        private async Task<object> CreateTokenObjectAsync(LocalParksUserModel user)
        {
            var model = await _service.GetUserTokenAsync(user);

            return new
            {
                token = model.Token,
                expiry = model.Expiry
            };
        }
    }
}
