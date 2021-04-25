using LocalParks.Core.Contracts;
using LocalParks.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LocalParks.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParksController : ControllerBase
    {
        private readonly ILogger<ParksController> _logger;
        private readonly IParksService _service;

        public ParksController(ILogger<ParksController> logger,
            IParksService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("api/[controller]")]
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

        [HttpGet("api/[controller]/{parkId:int}")]
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
        [HttpGet("api/[controller]/{parkName}")]
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
    }
}
