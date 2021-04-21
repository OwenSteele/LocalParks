using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalParks.Services.View
{
    public interface ISelectListService
    {
        Task<IEnumerable<SelectListItem>> GetParkSelectListItemsAsync<T>(bool withChildrenOnly = false) where T : class;
        Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync();
        IEnumerable<SelectListItem> GetSortSelectListItems<T>();
    }
}