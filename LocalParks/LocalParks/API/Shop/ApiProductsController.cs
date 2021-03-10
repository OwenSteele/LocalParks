using LocalParks.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/shop/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IShopService _service;
        private readonly IAuthenticationService _authenticationService;

        public ProductsController(ILogger<ProductsController> logger, IShopService service,
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _service = service;
            _authenticationService = authenticationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("API GET request: All Products");

            try
            {
                var results = await _service.GetAllProductsAsync();

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting all Products: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
