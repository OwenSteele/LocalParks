using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalParks.Services.View
{
    public interface ISelectListService
    {
        IEnumerable<SelectListItem> GetSortSelectListItems<T>();
    }
}