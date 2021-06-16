using LocalParks.API.Controllers.Base;
using LocalParks.Core.Contracts;
using LocalParks.Core.Models.Shop;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.API.Controllers.Shop
{
    public class OrdersController : AuthorizedControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("api/shop/[controller]")]
        public async Task<IActionResult> GetUserOrders()
        {
            var results = await _service.GetUserOrdersAsync(User.Identity.Name);

            return Ok(results);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("api/shop/[controller]/{orderId:int}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var results = await _service.GetOrderAsync(orderId);

            return Ok(results);
        }
        [HttpPost("api/shop/[controller]")]
        public async Task<IActionResult> Post(
            [FromBody] OrderModel model,
            [FromServices] IShopManager manager)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await manager.SubmitOrderAsync(model, User.Identity.Name);

            return Created($"api/shop/orders/{result.OrderId}", result);
        }
    }
}
