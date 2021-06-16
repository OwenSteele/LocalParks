using LocalParks.API.Controllers.Base;
using LocalParks.Core.Contracts;
using LocalParks.Core.Contracts.Managers;
using LocalParks.Core.Models.Account;
using LocalParks.Infrastructure.Handlers.ApiExceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.API.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }
        [HttpPost("[controller]/CreateToken")]
        public async Task<IActionResult> CreateToken(
            [FromBody] LoginModel model)
        {
            var result = await _authenticationManager.AuthenticateAsync(model);

            return Created("", result);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("api/[controller]/GetShopToken")]
        public async Task<IActionResult> GetShopToken()
        {
            var result = await _authenticationManager.AuthenticateContextAsync();

            return Created("", result);
        }
    }
}
