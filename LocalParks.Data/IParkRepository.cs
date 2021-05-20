using LocalParks.Core.Domain;
using LocalParks.Core.Domain.Shop;
using LocalParks.Core.Domain.User;
using System;
using System.Threading.Tasks;

namespace LocalParks.Data
{
    public interface IParkRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<Postcode[]> GetAllPostcodesAsync(bool includeChildren = true);
        Task<Postcode> GetPostcodeByZoneAsync(string Zone);

        Task<Park[]> GetAllParksAsync(bool includeChildren = true);
        Task<Park> GetParkByIdAsync(int parkId);
        Task<Park> GetParkByNameAsync(string parkName);
        Task<Park[]> GetParksByPostcodeAsync(string postcode);

        Task<SportsClub[]> GetAllSportsClubsAsync(bool includeChildren = true);
        Task<SportsClub[]> GetSportsClubsByParkIdAsync(int parkId);
        Task<SportsClub> GetSportsClubByIdAsync(int sportsClubId);
        Task<SportsClub[]> GetSportsClubsBySportAsync(SportType sport, int? parkId = null);
        Task<SportsClub> GetLatestSportsClubAsync();

        Task<Supervisor[]> GetAllSupervisorsAsync(bool includeChildren = true);
        Task<Supervisor> GetSupervisorByIdAsync(int employeeId);
        Task<Supervisor> GetSupervisorByParkIdAsync(int parkId);

        Task<ParkEvent[]> GetAllEventsAsync(bool includeChildren = true);
        Task<ParkEvent> GetEventByIdAsync(int eventId);
        Task<ParkEvent[]> GetEventsByParkIdAsync(int parkId);
        Task<ParkEvent> GetEventByParkIdAsync(int parkId, int eventId);
        Task<ParkEvent[]> GetEventsByDateAsync(DateTime dateTime);
        Task<ParkEvent[]> GetEventsUpToDateAsync(DateTime dateTime);
        Task<ParkEvent> GetEventByParkIdByDateAsync(int parkId, DateTime dateTime);
        Task<ParkEvent> GetLatestEventAsync();

        Task<LocalParksUser> GetLocalParksUserByUsernameAsync(string username);
        Task<LocalParksUser> GetLocalParksUserByEmailAsync(string email);

        Task<Order[]> GetAllOrdersAsync();
        Task<Order[]> GetOrdersByUsernameAsync(string username);
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> GetOrderByNumberAsync(string number);

        Task<Product[]> GetAllProductsAsync();
        Task<Product[]> GetShopProductsAsync();
        Task<Product[]> GetMembershipProductsAsync();
    }
}
