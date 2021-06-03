using LocalParks.Infrastructure.Handlers.ApiExceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.API.Controllers.Base
{
    [ApiController]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthorizedControllerBase: ControllerBase
    {        
    }
}
