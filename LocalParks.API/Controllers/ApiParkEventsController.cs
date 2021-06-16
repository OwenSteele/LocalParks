using LocalParks.API.Controllers.Base;
using LocalParks.Core.Contracts;
using LocalParks.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LocalParks.API.Controllers
{
    public class ParkEventsController : AuthorizedControllerBase
    {
        private readonly IParkEventsService _service;

        public ParkEventsController(IParkEventsService service)
        {
            _service = service;
        }

        [HttpGet("api/[controller]")]
        public async Task<ActionResult<ParkEventModel[]>> GetAllParkEvents()
        {
            var results = await _service.GetAllParkEventModelsAsync();

            return Ok(results);
        }

        [HttpGet("api/parks/{parkId:int}/[controller]")]
        public async Task<ActionResult<ParkEventModel[]>> GetParkEvents(int parkId)
        {
            var results = await _service.GetParkEventModelsByParkIdAsync(parkId);

            return Ok(results);
        }

        [HttpGet("api/[controller]/{eventId:int}")]
        public async Task<ActionResult<ParkEventModel>> GetEvent(int eventId)
        {
            var result = await _service.GetParkEventModelByIdAsync(eventId);

            return Ok(result);
        }

        [HttpGet("api/parks/{parkId:int}/[controller]/{eventId:int}")]
        public async Task<ActionResult<ParkEventModel>> GetEvent(int parkId, int eventId)
        {
            var result = await _service.GetParkEventModelByIdAsync(eventId, parkId);

            return Ok(result);
        }

        [HttpGet("api/parks/{parkId:int}/[controller]/{date:DateTime}")]
        public async Task<ActionResult<ParkEventModel>> GetEventByParkIdByDate(int parkId, DateTime date)
        {
            var result = await _service.GetParkEventModelAsync(parkId, date);

            return Ok(result);
        }
    }
}
