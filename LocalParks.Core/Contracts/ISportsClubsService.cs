using LocalParks.Core.Domain;
using LocalParks.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts
{
    public interface ISportsClubsService
    {
        Task<SportsClubModel[]> GetAllSportsClubModelsAsync();
        Task<SportsClubModel[]> GetSearchedSportsClubModelsAsync(string searchTerm = null, string parkId = null, string sportType = null);
        Task<IEnumerable<SelectListItem>> GetSportListItems();
        Task<SportsClubModel> GetSportsClubModelAsync(int clubId);
        Task<SportsClubModel[]> GetSportsClubModelsByParkAsync(int parkId);
        Task<SportsClubModel[]> GetSportsClubModelsBySportAsync(int parkId, string sport);
        Task<bool> CheckParkExistsAsync(int parkId, string clubName = null);
        ICollection<string> GetAllSports();
    }
}