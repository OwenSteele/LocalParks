using LocalParks.API.Controllers.Base;
using LocalParks.Core.Contracts;
using LocalParks.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.API.Controllers
{
    public class PostCodesController : AuthorizedControllerBase
    {
        private readonly IPostcodesService _service;

        public PostCodesController(IPostcodesService service)
        {
            _service = service;
        }

        [HttpGet("api/[controller]")]
        public async Task<ActionResult<PostcodeModel[]>> GetAllPostcodes()
        {
            var results = await _service.GetAllPostcodesAsync();

            return Ok(results);
        }

        [HttpGet("api/[controller]/{postcodeZone}")]
        public async Task<ActionResult<ParkModel>> GetPostcodeByZone(string postcodeZone)
        {
            var results = await _service.GetPostcodeAsync(postcodeZone);

            return Ok(results);
        }
    }
}
