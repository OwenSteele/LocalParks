using LocalParks.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IParksService
    {
        Task<ParkModel[]> GetAllModelsAsync();
        Task<ParkModel> GetParkAsync(int parkId);
        Task<ParkModel> GetParkAsync(string parkName);
        Task<ParkModel[]> GetSearchedAsync(string searchTerm = null, string postcode = null);
        Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync();
    }
}
