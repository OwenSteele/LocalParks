using LocalParks.Models;
using System.Threading.Tasks;

namespace LocalParks.Services.Admin
{
    public interface ISportsClubsAdminService
    {
        Task<SportsClubModel> AddNewSportsClubAsync(SportsClubModel model);
        Task<bool> DeleteSportsClubAsync(SportsClubModel model);
        Task<SportsClubModel> UpdateSportsClubAsync(SportsClubModel model);
    }
}