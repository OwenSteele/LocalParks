using LocalParks.Core.Models;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts
{
    public interface ISupervisorsService
    {
        Task<SupervisorModel[]> GetAllSupervisorModelsAsync();
        Task<SupervisorModel[]> GetSearchedSupervisorModelsAsync(string searchTerm = null, string parkId = null);
        Task<SupervisorModel> GetSupervisorModelAsync(int parkId, bool UseParkId = true);
        Task<bool> CheckParkExistsAsync(int parkId, bool IfHasSupervisorReturnFalse = false);
    }
}