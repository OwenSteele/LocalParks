using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LocalParks.API
{
    [ApiController]
    public class LPSportsClubsController : ControllerBase
    {
        private readonly ILogger<LPSportsClubsController> _logger;
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public LPSportsClubsController(ILogger<LPSportsClubsController> logger,
            IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<ActionResult<SportsClubModel[]>> GetAllSportsClubs()
        {
            _logger.LogInformation("API GET request: All Sports Clubs");

            try
            {
                var results = await _parkRepository.GetAllSportsClubsAsync();

                if (results == null) return NoContent();

                return Ok(_mapper.Map<SportsClubModel[]>(results));
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
                var results = await _parkRepository.GetSportsClubsByParkIdAsync(parkId);

                if (results == null) return NoContent();

                return Ok(_mapper.Map<SportsClubModel[]>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting all Sports Clubs from park with ID: {parkId}: {ex.Message}");

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
                var result = await _parkRepository.GetSportsClubByIdAsync(clubId);

                if (result == null) return NoContent();

                return Ok(_mapper.Map<SportsClubModel>(result));
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
                var result = await _parkRepository.GetSportsClubByIdAsync(clubId, parkId);

                if (result == null) return NoContent();

                return Ok(_mapper.Map<SportsClubModel>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting Sports Club with park ID '{clubId}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [Route("api/[controller]/{sport}")]
        [HttpGet("{sport}")]
        public async Task<ActionResult<SportsClubModel[]>> GetSportsClub(string sport)
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

                var result = await _parkRepository.GetSportsClubsBySportAsync(sportType);

                if (result == null) return NoContent();

                return Ok(_mapper.Map<SportsClubModel[]>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error occured in getting Sports Clubs with sport '{sport}': {ex.Message}");

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

                var result = await _parkRepository.GetSportsClubsBySportAsync(sportType, parkId);

                if (result == null) return NoContent();

                return Ok(_mapper.Map<SportsClubModel[]>(result));
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
