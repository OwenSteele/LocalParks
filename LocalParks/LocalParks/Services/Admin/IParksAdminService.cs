using LocalParks.Models;
using System.Threading.Tasks;

namespace LocalParks.Services.Admin
{
    public interface IParksAdminService
    {
        Task<ParkModel> AddParkAsync(ParkModel model);
        Task<bool> DeleteParkAsync(ParkModel model);
        Task<ParkModel> UpdateParkAsync(ParkModel model);
    }
}