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

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
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
    }
}
