using LocalParks.Core.Domain;
using LocalParks.Core.Domain.Shop;
using LocalParks.Core.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Data
{
    public class ParkRepository : IParkRepository
    {
        private readonly ParkContext _context;
        private readonly ILogger<ParkRepository> _logger;

        public ParkRepository(ParkContext context, ILogger<ParkRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type: {entity.GetType()} to the context.");
            _context.Add(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type: {entity.GetType()} to the context.");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempting to save changes to the context.");

            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Postcode> GetPostcodeByZoneAsync(string zone)
        {
            _logger.LogInformation($"Getting postcode of zone: {zone}.");

            IQueryable<Postcode> query = _context.Postcodes
                 .Include(z => z.Parks)
                 .Where(z => z.Zone.ToLower() == zone.ToLower());

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Postcode[]> GetAllPostcodesAsync(bool includeChildren = true)
        {
            _logger.LogInformation($"Getting all postcodes.");

            if (includeChildren)
            {
                return await _context.Postcodes.Include(c => c.Parks).OrderByDescending(z => z.Zone).ToArrayAsync();
            }
            return await _context.Postcodes.OrderByDescending(z => z.Zone).ToArrayAsync();
        }
        public async Task<Park[]> GetAllParksAsync(bool includeChildren = true)
        {
            _logger.LogInformation($"Getting all parks.");

            if (includeChildren)
            {
                return await _context.Parks
                .Include(p => p.Supervisor)
                .Include(p => p.SportClubs)
                .Include(p => p.Events)
                .OrderByDescending(p => p.Postcode)
                .ToArrayAsync();
            }

            return await _context.Parks
                .OrderByDescending(p => p.Postcode)
                .ToArrayAsync();
        }
        public async Task<Park> GetParkByIdAsync(int parkId)
        {
            _logger.LogInformation($"Getting park with ID: {parkId}.");

            return await _context.Parks
                .Include(p => p.Postcode)
                .Include(p => p.Supervisor)
                .Include(p => p.SportClubs)
                .Include(p => p.Events)
                .Where(p => p.ParkId == parkId)
                .FirstOrDefaultAsync();
        }
        public async Task<Park> GetParkByNameAsync(string parkName)
        {
            _logger.LogInformation($"Getting park with name: {parkName}.");

            return await _context.Parks
                .Include(p => p.Postcode)
                .Include(p => p.Supervisor)
                .Include(p => p.SportClubs)
                .Include(p => p.Events)
                .Where(p => p.Name.ToLower() == parkName.ToLower())
                .FirstOrDefaultAsync();
        }
        public async Task<Park[]> GetParksByPostcodeAsync(string postcodeZone)
        {
            _logger.LogInformation($"Getting parks in postcode zone: {postcodeZone}.");

            return await _context.Parks
                .Include(p => p.Postcode)
                .Include(p => p.Supervisor)
                .Include(p => p.SportClubs)
                .Include(p => p.Events)
                .Where(p => p.Postcode.Zone == postcodeZone)
                .ToArrayAsync();
        }

        public async Task<SportsClub[]> GetAllSportsClubsAsync(bool includeChildren = true)
        {
            _logger.LogInformation($"Getting all sports clubs.");

            if (includeChildren)
            {
                return await _context.SportsClubs.Include(c => c.Park)
                    .OrderByDescending(s => s.Sport)
                .ToArrayAsync();
            }

            return await _context.SportsClubs.Include(c => c.Park.ParkId)
                .OrderByDescending(s => s.Sport)
            .ToArrayAsync();
        }
        public async Task<SportsClub[]> GetSportsClubsByParkIdAsync(int parkId)
        {
            _logger.LogInformation($"Getting sports clubs from park ID: {parkId}.");

            return await _context.SportsClubs
                .Include(c => c.Park)
                .Where(c => c.Park.ParkId == parkId)
                .ToArrayAsync();
        }
        public async Task<SportsClub> GetSportsClubByIdAsync(int sportsClubId)
        {
            _logger.LogInformation($"Getting a sports club with ID: {sportsClubId}.");

            return await _context.SportsClubs
                .Include(c => c.Park)
                .Where(c => c.ClubId == sportsClubId)
                .FirstOrDefaultAsync();
        }
        public async Task<SportsClub[]> GetSportsClubsBySportAsync(SportType sport, int? parkId = null)
        {
            _logger.LogInformation($"Getting sports clubs with sport: {sport}.");

            IQueryable<SportsClub> query = _context.SportsClubs
                .Include(c => c.Park)
                .Where(s => s.Sport == sport)
                .OrderByDescending(s => s.Sport)
                .Where(c => parkId.HasValue && c.Park.ParkId == parkId);

            if (!query.Any()) return null;

            return await query.ToArrayAsync();
        }


        public async Task<Supervisor[]> GetAllSupervisorsAsync(bool includeChildren = true)
        {
            _logger.LogInformation($"Getting all supervisors.");

            if (includeChildren)
            {
                return await _context.Supervisors.Include(c => c.Park)
                .OrderByDescending(s => s.SupervisorId)
                .ToArrayAsync();
            }

            return await _context.Supervisors
                .OrderByDescending(s => s.SupervisorId)
                .ToArrayAsync();
        }
        public async Task<Supervisor> GetSupervisorByIdAsync(int employeeId)
        {
            _logger.LogInformation($"Getting the supervisor for ID: {employeeId}.");

            return await _context.Supervisors
                .Include(s => s.Park)
                .Where(s => s.SupervisorId == employeeId)
                .FirstOrDefaultAsync();
        }
        public async Task<Supervisor> GetSupervisorByParkIdAsync(int parkId)
        {
            _logger.LogInformation($"Getting the supervisor for park ID: {parkId}.");

            return await _context.Supervisors
                .Include(s => s.Park)
                .Where(s => s.Park.ParkId == parkId)
                .FirstOrDefaultAsync();
        }


        public async Task<ParkEvent[]> GetAllEventsAsync(bool includeChildren = true)
        {
            _logger.LogInformation($"Getting all events.");

            if (includeChildren)
            {
                return await _context.Events
                    .Include(c => c.Park)
                    .Include(e => e.User)
                    .Where(e => e.Date >= DateTime.Today)
                .OrderByDescending(e => e.Date).ToArrayAsync();
            }

            return await _context.Events
                .Where(e => e.Date >= DateTime.Today)
            .OrderByDescending(e => e.Date).ToArrayAsync();
        }
        public async Task<ParkEvent[]> GetEventsByParkIdAsync(int parkId)
        {
            _logger.LogInformation($"Getting events from park ID: {parkId}.");

            return await _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .Where(e =>
                    e.Park.ParkId == parkId &&
                    e.Date >= DateTime.Today)
                .OrderByDescending(e => e.Date)
                .ToArrayAsync();
        }
        public async Task<ParkEvent> GetEventByIdAsync(int eventId)
        {
            _logger.LogInformation($"Getting events from park ID: {eventId}.");

            return await _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .Where(e =>
            e.EventId == eventId &&
            e.Date >= DateTime.Today)
                .OrderByDescending(e => e.Date)
                .FirstOrDefaultAsync();
        }
        public async Task<ParkEvent> GetEventByParkIdAsync(int parkId, int eventId)
        {
            _logger.LogInformation($"Getting an event with ID: {eventId} from park ID: {parkId}.");

            return await _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .Where(e =>
            e.EventId == eventId &&
            e.Park.ParkId == parkId &&
            e.Date >= DateTime.Today)
                .FirstOrDefaultAsync();
        }
        public async Task<ParkEvent> GetEventByParkIdByDateAsync(int parkId, DateTime dateTime)
        {
            _logger.LogInformation(
                $"Getting an event from park ID: {parkId} on date: {dateTime.ToShortDateString()}.");

            return await _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .Where(e =>
            e.Date == dateTime &&
            e.Park.ParkId == parkId &&
            e.Date >= DateTime.Today)
                .FirstOrDefaultAsync();
        }
        public async Task<ParkEvent[]> GetEventsByDateAsync(DateTime dateTime)
        {
            _logger.LogInformation($"Getting all events.");

            return await _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .Where(e =>
            e.Date == dateTime &&
            e.Date >= DateTime.Today)
                .OrderByDescending(e => e.Name)
                .ToArrayAsync();
        }
        public async Task<ParkEvent[]> GetAllPastEventsAsync()
        {
            _logger.LogInformation($"Getting all events.");

            return await _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .OrderByDescending(e => e.Date)
                .ToArrayAsync();
        }

        public async Task<LocalParksUser> GetLocalParksUserByUsernameAsync(string username)
        {
            _logger.LogInformation($"Getting user by username");

            // no support for recursive queries - parks need to be included in services
            return await _context.Users
                .Include(u => u.OrganisedEvents)
                .ThenInclude(e => e.Park)
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();
        }
        public async Task<LocalParksUser> GetLocalParksUserByEmailAsync(string email)
        {
            _logger.LogInformation($"Getting user by username");

            return await _context.Users
                .Include(u => u.OrganisedEvents)
                .ThenInclude(e => e.Park)
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<Product[]> GetAllProductsAsync()
        {
            _logger.LogInformation($"Getting all products.");

            return await _context.Products
                .OrderByDescending(p => p.ProductId)
                .ToArrayAsync();
        }
        public async Task<Product[]> GetShopProductsAsync()
        {
            _logger.LogInformation($"Getting shop products.");

            return await _context.Products
                .Where(p => p.Category != ProductCategoryType.ClubMembership)
                .OrderByDescending(p => p.ProductId)
                .ToArrayAsync();
        }
        public async Task<Product[]> GetMembershipProductsAsync()
        {
            _logger.LogInformation($"Getting all products.");

            return await _context.Products
                .Where(p => p.Category == ProductCategoryType.ClubMembership)
                .OrderByDescending(p => p.ProductId)
                .ToArrayAsync();
        }
        public async Task<Order[]> GetAllOrdersAsync()
        {
            _logger.LogInformation($"Getting all order.");

            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.DateCreated)
                .ToArrayAsync();
        }
        public async Task<Order[]> GetOrdersByUsernameAsync(string username)
        {
            _logger.LogInformation($"Getting all order of user: {username}.");

            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User != null && o.User.UserName == username)
                .OrderByDescending(e => e.DateCreated)
                .ToArrayAsync();
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            _logger.LogInformation($"Getting order with Id: {id}.");

            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.OrderId == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Order> GetOrderByNumberAsync(string number)
        {
            _logger.LogInformation($"Getting order with Number: {number}.");

            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.OrderNumber == number)
                .FirstOrDefaultAsync();
        }

        public async Task<SportsClub> GetLatestSportsClubAsync()
        {
            _logger.LogInformation($"Getting Last added Sports Club.");

            return await _context.SportsClubs
                .OrderByDescending(c => c.ClubId)
                .FirstOrDefaultAsync();
        }

        public async Task<ParkEvent[]> GetEventsUpToDateAsync(DateTime dateTime)
        {
            _logger.LogInformation($"Getting all events up to date {dateTime}.");

            return await _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .Where(e =>
            e.Date <= dateTime &&
            e.Date >= DateTime.Today)
                .OrderByDescending(e => e.Name)
                .ToArrayAsync();
        }

        public async Task<ParkEvent> GetLatestEventAsync()
        {
            _logger.LogInformation($"Getting Last added event.");

            return await _context.Events
                .Where(e => e.Date >= DateTime.Today)
                .OrderByDescending(e => e.EventId)
                .FirstOrDefaultAsync();
        }
    }
}
