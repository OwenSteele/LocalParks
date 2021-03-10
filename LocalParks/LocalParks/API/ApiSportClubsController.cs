using LocalParks.Core;
using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LocalParks.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SportsClubsController : ControllerBase
    {
        private readonly ILogger<SportsClubsController> _logger;
        private readonly ISportsClubsService _service;
        private readonly IAuthenticationService _authenticationService;

        public SportsClubsController(ILogger<SportsClubsController> logger, ISportsClubsService service,
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _service = service;
            _authenticationService = authenticationService;
        }

        [Route("api/[controller]")]
        [HttpGet]
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

        [Route("api/parks/{parkId:int}/[controller]")]
        [HttpGet("{parkId:int}")]
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
        [Route("api/[controller]/sports")]
        [HttpGet("{parkId:int}")]
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

        [Route("api/[controller]/{clubId:int}")]
        [HttpGet("{clubId:int}")]
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

        [Route("api/parks/{parkId:int}/[controller]/{clubId:int}")]
        [HttpGet("{parkId:int},{clubId:int}")]
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

        [Route("api/parks/{parkId:int}/[controller]/{sport}")]
        [HttpGet("{parkId:int},{sport}")]
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
        [Route("api/[controller]")]
        [HttpPost]
        public async Task<ActionResult<SportsClubModel>> AddNewSportsClub(SportsClubModel model)
        {
            if (!await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
                return StatusCode(StatusCodes.Status403Forbidden);

            try
            {
                if (!model.ClubId.Equals(0)) return BadRequest("The 'ClubId' cannot be set, remove this property from model or set value to 0.");

                if (!_service.GetAllSports().Contains(model.Sport)) return BadRequest("Sport not found.");

                if (!await _service.CheckParkExistsAsync(model.ParkId, model.Name))
                    return BadRequest("Cannot add sports club to park with this parkId/club name.");

                if (await _service.GetSportsClubModelAsync(model.ClubId) != null)
                    return BadRequest("A sports club with this Id for this park already exists.");

                var result = await _service.AddNewSportsClubAsync(model);
                if (result == null) return BadRequest("No changes were made.");

                return Created("", result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [Route("api/[controller]/{clubId:int}")]
        [HttpPut]
        public async Task<ActionResult<SportsClubModel>> UpdateSportsClub(int clubId, SportsClubModel model)
        {
            if (!await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
                return StatusCode(StatusCodes.Status403Forbidden);

            try
            {
                if (!model.ClubId.Equals(0) && !model.ClubId.Equals(clubId))
                    return BadRequest("Must include the 'clubId' in query and must if included in body (cannot be edited).");

                if (model.ParkId.Equals(0)) return BadRequest("Must include 'parkId'.");

                if (!_service.GetAllSports().Contains(model.Sport)) return BadRequest("Sport not found.");

                if (!await _service.CheckParkExistsAsync(model.ParkId, model.Name))
                    return BadRequest("Cannot add sports club to park with this parkId/club name.");

                if (model.ClubId.Equals(0)) model.ClubId = clubId;

                var result = await _service.UpdateSportsClubAsync(model);
                if (result == null) return BadRequest("No changes were made.");

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [Route("api/[controller]/{clubId:int}")]
        [HttpDelete]
        public async Task<ActionResult<SportsClubModel>> DeleteSportsClub(int clubId)
        {
            if (!await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
                return StatusCode(StatusCodes.Status403Forbidden);

            try
            {
                var existing = await _service.GetSportsClubModelAsync(clubId);
                if (existing == null) return BadRequest("Sports Club not found.");

                if (await _service.DeleteSportsClubAsync(existing)) return Ok();

                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
