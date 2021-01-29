using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LocalParks.API
{
    [ApiController]
    public class ParkEventsController : ControllerBase
    {
        private readonly ILogger<ParkEventsController> _logger;
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public ParkEventsController(ILogger<ParkEventsController> logger,
            IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<ActionResult<ParkEventModel[]>> GetAllSportsClubs()
        {
            _logger.LogInformation("API GET request: All Events");

            try
            {
                var results = await _parkRepository.GetAllEventsAsync();

                return Ok(_mapper.Map<ParkEventModel[]>(results));
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
                var results = await _parkRepository.GetEventsByParkIdAsync(parkId);

                return Ok(_mapper.Map<ParkEventModel[]>(results));
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
                var result = await _parkRepository.GetEventByIdAsync(eventId);

                if (result == null) return NoContent();

                return Ok(_mapper.Map<ParkEventModel>(result));
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
                var result = await _parkRepository.GetEventByParkIdAsync(parkId,eventId);

                if (result == null) return NoContent();

                return Ok(_mapper.Map<ParkEventModel>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting Event with park ID '{eventId}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [Route("api/[controller]/{date}")]
        [HttpGet("{date}")]
        public async Task<ActionResult<ParkEventModel[]>> GetEventsByDate(string date)
        {
            _logger.LogInformation($"API GET request: Event with sport: {date}");

            try
            {
                var requiredDateFormat = "yyyy-MM-dd";

                if (!DateTime.TryParseExact(date,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime eventDate))
                {
                    return BadRequest(new { requiredDateFormat });
                }

                var result = await _parkRepository.GetEventsByDateAsync(eventDate);

                if (result == null) return NoContent();

                return Ok(_mapper.Map<ParkEventModel[]>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting Event with date input '{date}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [Route("api/parks/{parkId:int}/[controller]/{date}")]
        [HttpGet("{parkId:int},{date}")]
        public async Task<ActionResult<ParkEventModel>> GetSportsClub(int parkId, string date)
        {
            _logger.LogInformation($"API GET request: Sports Clubs with sport: {date}");

            try
            {
                var requiredDateFormat = "yyyy-MM-dd";

                if (!DateTime.TryParseExact(date,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime eventDate))
                {
                    return BadRequest(new { requiredDateFormat });
                }

                var result = await _parkRepository.GetEventByParkIdByDateAsync(parkId, eventDate);

                if (result == null) return NoContent();

                return Ok(_mapper.Map<ParkEventModel>(result));
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
