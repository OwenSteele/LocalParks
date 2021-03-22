using LocalParks.Models;
using LocalParks.Services.Shared;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface ISupervisorsService : IFilterService
    {
        Task<SupervisorModel[]> GetAllSupervisorModelsAsync();
        Task<SupervisorModel[]> GetSearchedSupervisorModelsAsync(string searchTerm = null, string parkId = null);
        Task<SupervisorModel> GetSupervisorModelAsync(int parkId, bool UseParkId = true);
        Task<bool> CheckParkExistsAsync(int parkId, bool IfHasSupervisorReturnFalse = false);
    }
}