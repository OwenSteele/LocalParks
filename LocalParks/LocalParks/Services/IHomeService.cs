using LocalParks.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IHomeService
    {
        Task<HomeModel> GetHomeModelAsync(string latitude, string longitude);
        Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync();
        Task<bool> PostFeedBackAsync(ContactModel model);
    }
}