using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.API
{
    [ApiController]
    public class SupervisorsController : ControllerBase
    {
        private readonly ILogger<SupervisorsController> _logger;
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public SupervisorsController(ILogger<SupervisorsController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<ActionResult<SupervisorModel[]>> GetAllSupervisors()
        {
            _logger.LogInformation("API GET request: All Supervisors");

            try
            {
                var results = await _parkRepository.GetAllSupervisorsAsync();

                if (results == null) return NoContent();

                return Ok(_mapper.Map<SupervisorModel[]>(results));
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
                var result = await _parkRepository.GetSupervisorByParkIdAsync(parkId);

                if (result == null) return NoContent();

                return Ok(_mapper.Map<SupervisorModel>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting Supervisor with park ID '{parkId}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
