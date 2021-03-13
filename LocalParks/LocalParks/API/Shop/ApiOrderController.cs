using LocalParks.Models.Shop;
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

namespace LocalParks.API.Shop
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/shop/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IShopService _service;

        public OrdersController(ILogger<OrdersController> logger, IShopService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpGet]
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
        [HttpGet]
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
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderModel model)
        {
            _logger.LogInformation($"API POST request: New Order");

            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = await _service.SubmitNewOrderAsync(model, User.Identity.Name);

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
