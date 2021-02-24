using LocalParks.Models;
using System;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IParkEventsService : IFilterService
    {
        Task<ParkEventModel> CreateNewEventAsync(ParkEventModel newEvent, ParkModel park);
        Task<ParkEventModel[]> GetAllParkEventModelsAsync(int? parkId = null, string sortBy = null);
        Task<ParkModel> GetParkAsync(int parkId);
        Task<ParkEventModel> GetParkEventModelAsync(int parkId, DateTime date);
        Task<ParkEventModel> GetParkEventModelByIdAsync(int eventId, int? parkId = null);
        Task<ParkEventModel[]> GetSearchedParkEventModelsAsync(string searchTerm = null, string parkId = null, DateTime? date = null, string sortBy = null);
        Task<bool> RemoveEventAsync(ParkEventModel parkEventModel);
    }
}