using LocalParks.Models;
using LocalParks.Services;
using LocalParks.Services.Admin;
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
    public class SupervisorsController : ControllerBase
    {
        private readonly ILogger<SupervisorsController> _logger;
        private readonly ISupervisorsService _service;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISupervisorsAdminService _adminService;

        public SupervisorsController(ILogger<SupervisorsController> logger,
            ISupervisorsService service,
            IAuthenticationService authenticationService,
            ISupervisorsAdminService adminService)
        {
            _logger = logger;
            _service = service;
            _authenticationService = authenticationService;
            _adminService = adminService;
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
        [Route("api/[controller]")]
        [HttpPost]
        public async Task<ActionResult<SupervisorModel>> AddNewSupervisor(SupervisorModel model)
        {
            if (!await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
                return StatusCode(StatusCodes.Status403Forbidden);

            try
            {
                if (!model.EmployeeId.Equals(0)) return BadRequest("The 'EmployeeId' cannot be set, remove this property from model or set value to 0.");

                if (!await _service.CheckParkExistsAsync(model.ParkId, true))
                    return BadRequest("Cannot add supervisor to park with this parkId.");

                var existing = await _service.GetSupervisorModelAsync(model.EmployeeId, false);
                if (existing != null) return BadRequest("A supervisor for this park already exists");

                var result = await _adminService.AddNewSupervisorAsync(model);
                if (result == null) return BadRequest("No changes were made.");

                return Created("", result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [Route("api/[controller]/{employeeId:int}")]
        [HttpPut]
        public async Task<ActionResult<SupervisorModel>> UpdateSupervisor(int employeeId, SupervisorModel model)
        {
            if (!await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
                return StatusCode(StatusCodes.Status403Forbidden);

            try
            {
                if (!model.EmployeeId.Equals(0) && !model.EmployeeId.Equals(employeeId)) return BadRequest("Must include the 'EmployeeId' in query and must if included in body (cannot be edited).");
                if (model.ParkId.Equals(0)) return BadRequest("Must include the unchanged 'parkId' to update.");

                if (model.EmployeeId.Equals(0)) model.EmployeeId = employeeId;

                var result = await _adminService.UpdateSupervisorAsync(model);
                if (result == null) return BadRequest("No changes were made.");

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
        [Route("api/[controller]/{employeeId:int}")]
        [HttpDelete]
        public async Task<ActionResult<SupervisorModel>> DeleteSupervisor(int employeeId)
        {
            if (!await _authenticationService.HasRequiredRoleAsync(this.User.Identity.Name, "Administrator"))
                return StatusCode(StatusCodes.Status403Forbidden);

            try
            {
                var existing = await _service.GetSupervisorModelAsync(employeeId, false);
                if (existing == null) return BadRequest("Supervisor not found.");

                if (await _adminService.DeleteSupervisorAsync(existing)) return Ok();

                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
