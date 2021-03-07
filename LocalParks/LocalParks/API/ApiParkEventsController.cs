using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace LocalParks.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParkEventsController : ControllerBase
    {
        private readonly ILogger<ParkEventsController> _logger;
        private readonly IParkEventsService _service;
        private readonly IAuthenticationService _authenticationService;

        public ParkEventsController(ILogger<ParkEventsController> logger, IParkEventsService service,
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _service = service;
            _authenticationService = authenticationService;
        }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<ActionResult<ParkEventModel[]>> GetAllParkEvents()
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
                    return BadRequest("Date format: 'yyyy-MM-dd'");
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

        [Route("api/[controller]")]
        [HttpPost]
        public async Task<ActionResult<ParkEventModel>> AddParkEventClub(ParkEventModel model)
        {
            try
            {
                if (!model.EventId.Equals(0)) return BadRequest("The 'ClubId' cannot be set, remove this property from model or set value to 0.");

                if (await _service.GetParkEventModelAsync(model.ParkId, model.Date) != null)
                    return BadRequest("Cannot create an event");

                var result = await _service.AddNewParkEventAsync(model, this.User.Identity.Name);
                if (result == null) return BadRequest("No changes were made.");

                return Created("", result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [Route("api/[controller]/{eventId:int}")]
        [HttpPut]
        public async Task<ActionResult<ParkEventModel>> UpdateParkEvent(int eventId, ParkEventModel model)
        {
            try
            {
                if (!await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
                {
                    if (await _service.GetEventOwner(eventId, this.User.Identity.Name) == null)
                        return StatusCode(StatusCodes.Status403Forbidden);
                }

                if (string.IsNullOrWhiteSpace(model.Username)) return BadRequest("Remove user setter.");

                if (!model.EventId.Equals(0) && !model.EventId.Equals(eventId))
                    return BadRequest("Must include the 'eventId' in query and must if included in body (cannot be edited).");

                if (model.ParkId.Equals(0)) return BadRequest("Must include 'parkId'.");

                if (await _service.GetParkAsync(model.ParkId) == null)
                    return BadRequest("No park with this Id");

                if (model.EventId.Equals(0)) model.EventId = eventId;                                                             

                var result = await _service.UpdateParkEventAsync(model);
                if (result == null) return BadRequest("No changes were made.");
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [Route("api/[controller]/{eventId:int}")]
        [HttpDelete]
        public async Task<ActionResult<ParkEventModel>> DeleteParkEvent(int eventId)
        {
            try
            {
                if (!await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
                {
                    if (await _service.GetEventOwner(eventId, this.User.Identity.Name) == null)
                        return StatusCode(StatusCodes.Status403Forbidden);
                }

                var existing = await _service.GetParkEventModelByIdAsync(eventId);
                if (existing == null) return BadRequest("Sports Club not found.");

                if (await _service.DeleteParkEventAsync(existing)) return Ok();

                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
