using LocalParks.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IParksService
    {
        Task<ParkModel> AddParkAsync(ParkModel model);
        Task<ParkModel[]> GetAllModelsAsync(string sortBy = null);
        Task<ParkModel> GetParkAsync(int parkId);
        Task<ParkModel> GetParkAsync(string parkName);
        Task<PostcodeModel> GetPostcodeAsync(string postcode);
        Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync();
        Task<ParkModel[]> GetSearchedAsync(string searchTerm = null, string postcode = null, string sortBy = null);
        IEnumerable<SelectListItem> GetSortSelectListItems();
    }
}