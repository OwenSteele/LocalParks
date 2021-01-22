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

        //Task<SportsClub[]> GetAllSportsClubsAsync();
        Task<SportsClub[]> GetSportsClubsByParkIdAsync(int parkId);
        Task<SportsClub> GetSportsClubByParkIdAsync(int parkId, int sportsClubId);
        //Task<SportsClub[]> GetSportsClubsBySportAsync(SportType sport);

        Task<Supervisor[]> GetAllSupervisorsAsync();
        Task<Supervisor> GetSupervisorByParkIdAsync(int parkId);

        //Task<Event[]> GetAllEventsAsync();
        Task<Event[]> GetEventsByParkIdAsync(int parkId);
        Task<Event> GetEventByParkIdAsync(int parkId, int eventId);
        //Task<Event[]> GetEventsByDateAsync(DateTime dateTime);
        Task<Event> GetEventByParkIdByDateAsync(int parkId, DateTime dateTime);
    }
}
