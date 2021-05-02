using LocalParks.Core.Contracts;
using LocalParks.Core.Models.Shop;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LocalParks.API.Controllers.Shop
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _service;

        public OrdersController(ILogger<OrdersController> logger, IOrderService service)
        {
            _logger = logger;
            _service = service;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("api/shop/[controller]")]
        public async Task<IActionResult> GetUserOrders()
        {
            _logger.LogInformation("API GET request: All Orders of user");

            try
            {
                var results = await _service.GetUserOrdersAsync(User.Identity.Name);

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting all Products: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("api/shop/[controller]/{orderId:int}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            _logger.LogInformation($"API GET request: get order with Id: {orderId} ");

            try
            {
                var results = await _service.GetOrderAsync(orderId);

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting all Products: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [HttpPost("api/shop/[controller]")]
        public async Task<IActionResult> Post(
            [FromBody] OrderModel model,
            [FromServices] IShopManager manager)
        {
            _logger.LogInformation($"API POST request: New Order");

            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = await manager.SubmitOrderAsync(model, User.Identity.Name);

                if (result == null) return BadRequest();

                return Created($"api/shop/orders/{result.OrderId}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in creating new order: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
