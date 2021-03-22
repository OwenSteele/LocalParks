using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services.Shared
{
    public interface IFilterService
    {
        Task<IEnumerable<SelectListItem>> GetParkSelectListItemsAsync(bool onlyWithChildren = false);
    }
}
