using LocalParks.Models;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface ISupervisorsService : IFilterService
    {
        Task<SupervisorModel[]> GetAllSupervisorModelsAsync(string sortBy = null);
        Task<SupervisorModel[]> GetSearchedSupervisorModelsAsync(string searchTerm, string parkId, string sortBy);
        Task<SupervisorModel> GetSupervisorModelAsync(int parkId, bool UseParkId = true);
        Task<bool> CheckParkExistsAsync(int parkId, bool IfHasSupervisorReturnFalse = false);
        Task<SupervisorModel> AddNewSupervisorAsync(SupervisorModel model);
        Task<SupervisorModel> UpdateSupervisorAsync(SupervisorModel model);
        Task<bool> DeleteSupervisorAsync(SupervisorModel model);
    }
}