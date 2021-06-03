using LocalParks.API.Controllers.Base;
using LocalParks.Core.Contracts;
using LocalParks.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.API.Controllers
{
    public class ParksController : AuthorizedControllerBase
    {
        private readonly IParksService _service;

        public ParksController(IParksService service)
        {
            _service = service;
        }

        [HttpGet("api/[controller]")]
        public async Task<ActionResult<ParkModel[]>> GetAllParks()
        {
            var results = await _service.GetAllModelsAsync();

            return Ok(results);
        }

        [HttpGet("api/[controller]/{parkId:int}")]
        public async Task<ActionResult<ParkModel>> GetParkById(int parkId)
        {
            var results = await _service.GetParkAsync(parkId);

            return Ok(results);
        }
        [HttpGet("api/[controller]/{parkName}")]
        public async Task<ActionResult<ParkModel>> GetParkByName(string parkName)
        {
            var results = await _service.GetSearchedAsync(parkName);

            return Ok(results);
        }
    }
}
