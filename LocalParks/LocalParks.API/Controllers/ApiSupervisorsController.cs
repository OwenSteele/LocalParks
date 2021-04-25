﻿using LocalParks.Core.Contracts;
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
    public class SupervisorsController : ControllerBase
    {
        private readonly ILogger<SupervisorsController> _logger;
        private readonly ISupervisorsService _service;

        public SupervisorsController(ILogger<SupervisorsController> logger,
            ISupervisorsService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("api/[controller]")]
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

        [HttpGet("api/[controller]/{parkId:int}")]
        [HttpGet("api/parks/{parkId:int}/[controller]")]
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
