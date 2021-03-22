using LocalParks.Models;
using LocalParks.Services.Shared;
using System;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IParkEventsService : IFilterService
    {
        Task<ParkEventModel[]> GetAllParkEventModelsAsync();
        Task<ParkModel> GetParkAsync(int parkId);
        Task<ParkEventModel> GetParkEventModelAsync(int parkId, DateTime date);
        Task<ParkEventModel[]> GetParkEventModelsByParkIdAsync(int parkId);
        Task<ParkEventModel> GetParkEventModelByIdAsync(int eventId, int? parkId = null);
        Task<ParkEventModel[]> GetSearchedParkEventModelsAsync(string searchTerm = null, string parkId = null, DateTime? date = null);
        Task<ParkEventModel> AddNewParkEventAsync(ParkEventModel model, string username, bool hideUsername = true);
        Task<ParkEventModel> UpdateParkEventAsync(ParkEventModel model);
        Task<bool> DeleteParkEventAsync(ParkEventModel model);
        Task<LocalParksUserModel> GetEventOwner(int eventId, string userName = null);
    }
}