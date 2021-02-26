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

        public AccountController(ILogger<AccountController> logger, IAccountService service)
        {
            _logger = logger;
            _service = service;
        }

        [Route("api/[controller]/CreateToken")]
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel model)
        {
            _logger.LogInformation("CreateToken Request ApiAccountController");

            if (!ModelState.IsValid) return BadRequest();

            var result = await _service.GetUserTokenAsync(model);

            return Created("", result);
        }
    }
}
