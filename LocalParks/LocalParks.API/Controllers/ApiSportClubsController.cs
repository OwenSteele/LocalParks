using LocalParks.Core.Contracts;
using LocalParks.Core.Domain;
using LocalParks.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LocalParks.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SportsClubsController : ControllerBase
    {
        private readonly ILogger<SportsClubsController> _logger;
        private readonly ISportsClubsService _service;

        public SportsClubsController(ILogger<SportsClubsController> logger,
            ISportsClubsService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("api/[controller]")]
        public async Task<ActionResult<SportsClubModel[]>> GetAllSportsClubs()
        {
            _logger.LogInformation("API GET request: All Sports Clubs");

            try
            {
                var results = await _service.GetAllSportsClubModelsAsync();

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting all Sports Clubs: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [HttpGet("api/parks/{parkId:int}/[controller]")]
        public async Task<ActionResult<SportsClubModel[]>> GetParkSportsClubs(int parkId)
        {
            _logger.LogInformation($"API GET request: All Sports Clubs in park with ID: {parkId}");

            try
            {
                var results = await _service.GetSportsClubModelsByParkAsync(parkId);

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting all Sports Clubs from park with ID: {parkId}: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [HttpGet("api/[controller]/sports")]
        public ActionResult<SportsClubModel[]> GetAllSports()
        {
            _logger.LogInformation($"API GET request: All Sports");

            try
            {
                var results = _service.GetAllSports();

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [HttpGet("api/[controller]/{clubId:int}")]
        public async Task<ActionResult<SportsClubModel>> GetSportsClub(int clubId)
        {
            _logger.LogInformation($"API GET request: Sports Club with ID: {clubId}");

            try
            {
                var result = await _service.GetSportsClubModelAsync(clubId);

                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting Sports Club with ID '{clubId}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [HttpGet("api/parks/{parkId:int}/[controller]/{clubId:int}")]
        public async Task<ActionResult<SportsClubModel>> GetSportsClub(int parkId, int clubId)
        {
            _logger.LogInformation($"API GET request: Sports Club with ID: {clubId}, in park with ID: {parkId}");

            try
            {
                var result = await _service.GetSportsClubModelAsync(clubId);

                if (result == null || result.ParkId != parkId) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting Sports Club with park ID '{clubId}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [HttpGet("api/parks/{parkId:int}/[controller]/{sport}")]
        public async Task<ActionResult<SportsClubModel[]>> GetSportsClub(int parkId, string sport)
        {
            _logger.LogInformation($"API GET request: Sports Clubs with sport: {sport}");

            try
            {
                if (!Enum.TryParse(sport, true, out SportType sportType))
                {
                    var validSports = Enum.GetNames(typeof(SportType));
                    var message = "Sport type does not exist";

                    return BadRequest(new { message, validSports });
                }

                var result = await _service.GetSportsClubModelsBySportAsync(parkId, sportType);

                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting Sports Clubs with sport '{sport}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
