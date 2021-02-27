using LocalParks.Core;
using LocalParks.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface ISportsClubsService : IFilterService
    {
        Task<SportsClubModel[]> GetAllSportsClubModelsAsync(string sortBy = null);
        Task<SportsClubModel[]> GetSearchedSportsClubModelsAsync(string searchTerm = null, string parkId = null, string sportType = null, string sortBy = null);
        IEnumerable<SelectListItem> GetSportListItems();
        Task<SportsClubModel> GetSportsClubModelAsync(int clubId, int? parkId = null);
        Task<SportsClubModel[]> GetSportsClubModelsByParkAsync(int parkId);
        Task<SportsClubModel[]> GetSportsClubModelsBySportAsync(int parkId, SportType sportType);
        Task<bool> CheckParkExistsAsync(int parkId, string clubName = null);
        Task<SportsClubModel> AddNewSportsClubAsync(SportsClubModel model);
        Task<SportsClubModel> UpdateSportsClubAsync(SportsClubModel model);
        Task<bool> DeleteSportsClubAsync(SportsClubModel model);
        ICollection<string> GetAllSports();
    }
}