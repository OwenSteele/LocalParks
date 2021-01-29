using LocalParks.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Data
{
    public interface IParkRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<Park[]> GetAllParksAsync();
        Task<Park> GetParkByIdAsync(int parkId);
        Task<Park> GetParkByNameAsync(string parkName);
        Task<Park[]> GetParksByPostcodeAsync(PostcodeType postcode);

        Task<SportsClub[]> GetAllSportsClubsAsync();
        Task<SportsClub[]> GetSportsClubsByParkIdAsync(int parkId);
        Task<SportsClub> GetSportsClubByIdAsync(int sportsClubId, int? parkId = null);
        Task<SportsClub[]> GetSportsClubsBySportAsync(SportType sport, int? parkId = null);

        Task<Supervisor[]> GetAllSupervisorsAsync();
        Task<Supervisor> GetSupervisorByParkIdAsync(int parkId);

        Task<ParkEvent[]> GetAllEventsAsync();
        Task<ParkEvent> GetEventByIdAsync(int eventId);
        Task<ParkEvent[]> GetEventsByParkIdAsync(int parkId);
        Task<ParkEvent> GetEventByParkIdAsync(int parkId, int eventId);
        Task<ParkEvent[]> GetEventsByDateAsync(DateTime dateTime);
        Task<ParkEvent> GetEventByParkIdByDateAsync(int parkId, DateTime dateTime);
    }
}
