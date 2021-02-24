using LocalParks.Models;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface ISupervisorsService : IFilterService
    {
        Task<SupervisorModel[]> GetAllSupervisorModelsAsync(string sortBy = null);
        Task<SupervisorModel[]> GetSearchedSupervisorModelsAsync(string searchTerm, string parkId, string sortBy);
        Task<SupervisorModel> GetSupervisorModelAsync(int parkId);
    }
}