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
    [Route("api/[controller]")]
    [ApiController]
    public class LPParksController : ControllerBase
    {
        private readonly ILogger<LPParksController> _logger;
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public LPParksController(ILogger<LPParksController> logger, IParkRepository parkRepository, IMapper mapper)
        {
            _logger = logger;
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ParkModel[]>> GetAllParks()
        {
            _logger.LogInformation("API GET request: All Parks");

            try
            {
                var results = await _parkRepository.GetAllParksAsync();

                if (results == null) return NoContent();

                return Ok(_mapper.Map<ParkModel[]>(results));
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
                var results = await _parkRepository.GetParkByIdAsync(parkId);

                if (results == null) return NoContent();

                return Ok(_mapper.Map<ParkModel>(results));
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
                var results = await _parkRepository.GetParkByNameAsync(parkName);

                if (results == null) return NoContent();

                return Ok(_mapper.Map<ParkModel>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in getting park with name '{parkName}': {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure");
            }
        }
    }
}
