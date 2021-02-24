using LocalParks.Models;
using LocalParks.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LocalParks.API
{
    [ApiController]
    public class SupervisorsController : ControllerBase
    {
        private readonly ILogger<SupervisorsController> _logger;
        private readonly ISupervisorsService _service;

        public SupervisorsController(ILogger<SupervisorsController> logger, ISupervisorsService service)
        {
            _logger = logger;
            _service = service;
        }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<ActionResult<SupervisorModel[]>> GetAllSupervisors()
        {
            _logger.LogInformation("API GET request: All Supervisors");

            try
            {
                var results = await _service.GetAllSupervisorModelsAsync();

                if (results == null) return NoContent();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting all Supervisors: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }

        [Route("api/parks/{parkId:int}/[controller]")]
        [Route("api/[controller]/{parkId:int}")]
        [HttpGet("{parkId:int}")]
        public async Task<ActionResult<SupervisorModel>> GetSupervisor(int parkId)
        {
            _logger.LogInformation($"API GET request: Supervisor with park ID: {parkId}");

            try
            {
                var result = await _service.GetSupervisorModelAsync(parkId);

                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting Supervisor with park ID '{parkId}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
