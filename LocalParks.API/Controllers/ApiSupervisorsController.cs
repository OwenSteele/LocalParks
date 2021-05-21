using LocalParks.API.Controllers.Base;
using LocalParks.Core.Contracts;
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
    public class SupervisorsController : AuthorizedControllerBase
    {
        private readonly ISupervisorsService _service;

        public SupervisorsController(ISupervisorsService service)
        {
            _service = service;
        }

        [HttpGet("api/[controller]")]
        public async Task<ActionResult<SupervisorModel[]>> GetAllSupervisors()
        {
            var results = await _service.GetAllSupervisorModelsAsync();

            return Ok(results);
        }

        [HttpGet("api/[controller]/{parkId:int}")]
        public async Task<ActionResult<SupervisorModel>> GetSupervisor(int parkId)
        {
            var result = await _service.GetSupervisorModelAsync(parkId);

            return Ok(result);
        }
    }
}
