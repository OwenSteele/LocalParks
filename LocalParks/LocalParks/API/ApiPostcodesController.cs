using AutoMapper;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostCodesController : ControllerBase
    {
        private readonly ILogger<PostCodesController> _logger;
        private readonly IPostcodesService _service;

        public PostCodesController(ILogger<PostCodesController> logger, IPostcodesService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PostcodeModel[]>> GetAllPostcodes()
        {
            _logger.LogInformation("API GET request: All Postcodes");

            try
            {
                var results = await _service.GetAllPostcodesAsync();

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting all parks: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [HttpGet("{postcodeZone}")]
        public async Task<ActionResult<ParkModel>> GetPostcodeByZone(string postcodeZone)
        {
            _logger.LogInformation($"API GET request: postcode with ID: {postcodeZone}");

            try
            {
                var results = await _service.GetPostcodeAsync(postcodeZone);

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting  postcode with ID: '{postcodeZone}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
