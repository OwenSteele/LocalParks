using LocalParks.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts
{
    public interface IParksService
    {
        Task<ParkModel[]> GetAllModelsAsync();
        Task<ParkModel> GetParkAsync(int parkId);
        Task<ParkModel> GetParkAsync(string parkName);
        Task<ParkModel[]> GetSearchedAsync(string searchTerm = null, string postcode = null, bool openOnly = false);
        Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync();
    }
}
