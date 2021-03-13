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

        [Route("api/[controller]/CreateToken")]
        public IActionResult CreateToken()
        {
            _logger.LogInformation("CreateToken Request ApiAccountController");

            return Ok(new
            {
                Error = new
                {
                    Code = "Endpoint no longer in use.",
                    Information = "This API function has been deprecated and is no longer usable.",
                    Message = "To improve security, sending user information is no longer recommended or supported with the Local Parks API.",
                    Redirect = new
                    {
                        Message = "Please log on to the Local Parks Website and generate a token.",
                        Path = "/Account/TokenGenerator",
                        URL = "https://localparks.azurewebsites.net/Account/TokenGenerator"
                    }
                }
            });
        }
        [Route("api/[controller]/GetShopToken")]
        public async Task<IActionResult> GetShopToken(string requestor)
        {
            _logger.LogInformation("GetShopToken Request ApiAccountController");

            if (!requestor.Equals("shop") ||
                this.User.Identity.Name == null ||
                !await _authenticationService.IsSignedIn(this.User))
                return Unauthorized();

            var user = await _service.GetUserAsync(this.User.Identity.Name);

            if (user == null) return BadRequest();

            var result = await _service.GetUserTokenAsync(user);

            return Created("", result);
        }
    }
}
