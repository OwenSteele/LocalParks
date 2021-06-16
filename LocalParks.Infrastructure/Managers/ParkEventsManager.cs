using LocalParks.Core.Contracts;
using LocalParks.Core.Contracts.Managers;
using LocalParks.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Infrastructure.Managers
{
    public class ParkEventsManager : IParkEventsManager
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IParkEventsService _parkEventsService;

        public ParkEventsManager(IAuthenticationService authenticationService,
            IParkEventsService parkEventsService
            )
        {
            _authenticationService = authenticationService;
            _parkEventsService = parkEventsService;
        }
        public async Task<bool> IsUserAuthorizedAsync(string username, ParkEventModel @event)
        {
            if (await _authenticationService.HasRequiredRoleAsync(username, "Administrator"))
                return true;

            return await _parkEventsService.GetEventOwner(@event.EventId, username) != null;
        }
    }
}
