using LocalParks.Core.Models;
using System.Threading.Tasks;

namespace LocalParks.Infrastructure.Services.Admin
{
    public interface ISupervisorsAdminService
    {
        Task<SupervisorModel> AddNewSupervisorAsync(SupervisorModel model);
        Task<bool> DeleteSupervisorAsync(SupervisorModel model);
        Task<SupervisorModel> UpdateSupervisorAsync(SupervisorModel model);
    }
}