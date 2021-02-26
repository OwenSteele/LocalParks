using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LocalParks.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParksController : ControllerBase
    {
        private readonly ILogger<ParksController> _logger;
        private readonly IParksService _service;

        public ParksController(ILogger<ParksController> logger, IParksService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ParkModel[]>> GetAllParks()
        {
            _logger.LogInformation("API GET request: All Parks");

            try
            {
                var results = await _service.GetAllModelsAsync();

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting all parks: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [HttpGet("{parkId:int}")]
        public async Task<ActionResult<ParkModel>> GetParkById(int parkId)
        {
            _logger.LogInformation($"API GET request: park with ID: {parkId}");

            try
            {
                var results = await _service.GetParkAsync(parkId);

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting park with ID '{parkId}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [HttpGet("{parkName}")]
        public async Task<ActionResult<ParkModel>> GetParkByName(string parkName)
        {
            _logger.LogInformation($"API GET request: park with name: {parkName}");

            try
            {
                var results = await _service.GetSearchedAsync(parkName);

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting park with name '{parkName}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ParkModel>> AddNewPark(ParkModel model)
        {
            try
            {
                if (!model.ParkId.Equals(0)) return BadRequest("The 'parkId' cannot be set, remove this property from model or set value to 0.");

                var existing = await _service.GetParkAsync(model.Name);
                if (existing != null) return BadRequest("A park with this name already exists.");

                var postcode = await _service.GetPostcodeAsync(model.PostcodeZone);
                if (postcode == null) return BadRequest("Invalid Postcode.");

                var result = await _service.AddParkAsync(model);

                if (result == null) return BadRequest();

                return Created("", result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
