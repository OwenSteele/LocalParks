﻿using LocalParks.Authentication;
using LocalParks.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace LocalParks.API
{
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly TokenService _service;

        public TokenController(ILogger<TokenController> logger)
        {
            _logger = logger;
            _service = new();
        }

        [Route("api/[controller]")]
        [HttpGet]
        public IActionResult GetToken(string username, string password)
        {
            _logger.LogInformation($"API Token request [{username.ToCharArray()}]");

            if (_service.CheckUser(username, password))
            {
                _logger.LogInformation($"API Token granted [{username.ToCharArray()}]");

                return Ok(JwtManager.GenerateToken(username));
            }

            return StatusCode(StatusCodes.Status401Unauthorized);
        }
    }
}
