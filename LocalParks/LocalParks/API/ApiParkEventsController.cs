using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;
using LocalParks.Services;

namespace LocalParks.API
{
    [ApiController]
    public class ParkEventsController : ControllerBase
    {
        private readonly ILogger<ParkEventsController> _logger;
        private readonly ParkEventsService _service;

        public ParkEventsController(ILogger<ParkEventsController> logger,
            IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _service = new ParkEventsService(parkRepository, mapper);
        }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<ActionResult<ParkEventModel[]>> GetAllSportsClubs()
        {
            _logger.LogInformation("API GET request: All Events");

            try
            {
                var results = await _service.GetAllParkEventModelsAsync();
                
                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting all Events: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [Route("api/parks/{parkId:int}/[controller]")]
        [HttpGet("{parkId:int}")]
        public async Task<ActionResult<ParkEventModel[]>> GetParkEvents(int parkId)
        {
            _logger.LogInformation($"API GET request: All Events in park with ID: {parkId}");

            try
            {
                var results = await _service.GetAllParkEventModelsAsync(parkId);

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting all Events from park with ID: {parkId}: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [Route("api/[controller]/{eventId:int}")]
        [HttpGet("{eventId:int}")]
        public async Task<ActionResult<ParkEventModel>> GetEvent(int eventId)
        {
            _logger.LogInformation($"API GET request: Event with ID: {eventId}");

            try
            {
                var result = await _service.GetParkEventModelByIdAsync(eventId);

                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting Event with ID '{eventId}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [Route("api/parks/{parkId:int}/[controller]/{eventId:int}")]
        [HttpGet("{parkId:int},{eventId:int}")]
        public async Task<ActionResult<ParkEventModel>> GetEvent(int parkId, int eventId)
        {
            _logger.LogInformation($"API GET request: Event with ID: {eventId}, in park with ID: {parkId}");

            try
            {
                var result = await _service.GetParkEventModelByIdAsync(eventId, parkId);

                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting Event with park ID '{eventId}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [Route("api/parks/{parkId:int}/[controller]/{date}")]
        [HttpGet("{parkId:int},{date}")]
        public async Task<ActionResult<ParkEventModel>> GetEventByParkIdByDate(int parkId, string date)
        {
            _logger.LogInformation($"API GET request: Event By ParkId By Date: {date}");

            try
            {
                if (!DateTime.TryParseExact(date,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime eventDate))
                {
                    return BadRequest("yyyy-MM-dd");
                }

                var result = await _service.GetParkEventModelAsync(parkId, eventDate);

                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting Event with date input '{date}' in park with ID {parkId}: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
