using LocalParks.Core.Contracts;
using LocalParks.Infrastructure.Handlers.ApiExceptions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.API.Controllers.Shop
{
    [ApiController]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }
        [HttpGet("api/shop/[controller]")]
        public async Task<IActionResult> GetProducts()
        {
            var results = await _service.GetShopProductsAsync();

            return Ok(results);
        }
        [HttpGet("api/shop/[controller]/memberships")]
        public async Task<IActionResult> GetMembership()
        {
            var results = await _service.GetMembershipProductsAsync();

            return Ok(results);
        }
    }
}
