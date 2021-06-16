using LocalParks.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts.Managers
{
    public interface IParkEventsManager
    {
        Task<bool> IsUserAuthorizedAsync(string username, ParkEventModel @event);
    }
}
