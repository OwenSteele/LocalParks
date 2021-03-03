using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    internal interface IFilterService
    {
        Task<IEnumerable<SelectListItem>> GetParkSelectListItemsAsync(bool onlyWithModels = false);
        IEnumerable<SelectListItem> GetSortSelectListItems();
    }
}