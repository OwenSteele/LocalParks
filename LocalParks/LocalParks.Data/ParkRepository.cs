using LocalParks.Core;
using LocalParks.Core.Shop;
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

            IQueryable<Postcode> query;

            if (includeChildren)
            {
                query = _context.Postcodes.Include(c => c.Parks);
            }
            else
            {
                query = _context.Postcodes;
            }

            query = query.OrderByDescending(z => z.Zone);

            return await query.ToArrayAsync();
        }
        public async Task<Park[]> GetAllParksAsync(bool includeChildren = true)
        {
            _logger.LogInformation($"Getting all parks.");

            IQueryable<Park> query;

            if (includeChildren)
            {
                query = _context.Parks
                .Include(p => p.Supervisor)
                .Include(p => p.SportClubs)
                .Include(p => p.Events);
            }
            else
            {
                query = _context.Parks;
            }

            query = query.OrderByDescending(p => p.Postcode);

            return await query.ToArrayAsync();
        }
        public async Task<Park> GetParkByIdAsync(int parkId)
        {
            _logger.LogInformation($"Getting park with ID: {parkId}.");

            IQueryable<Park> query = _context.Parks
                .Include(p => p.Supervisor)
                .Include(p => p.SportClubs)
                .Include(p => p.Events)
                .Where(p => p.ParkId == parkId);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Park> GetParkByNameAsync(string parkName)
        {
            _logger.LogInformation($"Getting park with name: {parkName}.");

            IQueryable<Park> query = _context.Parks
                .Include(p => p.Supervisor)
                .Include(p => p.SportClubs)
                .Include(p => p.Events)
                .Where(p => p.Name.ToLower() == parkName.ToLower());

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Park[]> GetParksByPostcodeAsync(string postcodeZone)
        {
            _logger.LogInformation($"Getting parks in postcode zone: {postcodeZone}.");

            IQueryable<Park> query = _context.Parks
                .Include(p => p.Supervisor)
                .Include(p => p.SportClubs)
                .Include(p => p.Events)
                .Where(p => p.Postcode.Zone == postcodeZone);

            return await query.ToArrayAsync();
        }

        public async Task<SportsClub[]> GetAllSportsClubsAsync(bool includeChildren = true)
        {
            _logger.LogInformation($"Getting all sports clubs.");

            IQueryable<SportsClub> query;

            if (includeChildren)
            {
                query = _context.SportsClubs.Include(c => c.Park);
            }
            else
            {
                query = _context.SportsClubs.Include(c => c.Park.ParkId);
            }

            query = query.OrderByDescending(s => s.Sport);

            return await query.ToArrayAsync();
        }
        public async Task<SportsClub[]> GetSportsClubsByParkIdAsync(int parkId)
        {
            _logger.LogInformation($"Getting sports clubs from park ID: {parkId}.");

            IQueryable<SportsClub> query = _context.SportsClubs
                .Include(c => c.Park)
                .Where(c => c.Park.ParkId == parkId);

            return await query.ToArrayAsync();
        }
        public async Task<SportsClub> GetSportsClubByIdAsync(int sportsClubId)
        {
            _logger.LogInformation($"Getting a sports club with ID: {sportsClubId}.");

            IQueryable<SportsClub> query = _context.SportsClubs
                .Include(c => c.Park)
                .Where(c => c.ClubId == sportsClubId);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<SportsClub[]> GetSportsClubsBySportAsync(SportType sport, int? parkId = null)
        {
            _logger.LogInformation($"Getting sports clubs with sport: {sport}.");

            IQueryable<SportsClub> query = _context.SportsClubs
                .Include(c => c.Park)
                .Where(s => s.Sport == sport)
                .OrderByDescending(s => s.Sport);

            if (parkId.HasValue) query = query.Where(c => c.Park.ParkId == parkId);

            if (!query.Any()) return null;

            return await query.ToArrayAsync();
        }


        public async Task<Supervisor[]> GetAllSupervisorsAsync(bool includeChildren = true)
        {
            _logger.LogInformation($"Getting all supervisors.");

            IQueryable<Supervisor> query;

            if (includeChildren)
            {
                query = _context.Supervisors.Include(c => c.Park);
            }
            else
            {
                query = _context.Supervisors;
            }

            query = query.OrderByDescending(s => s.SupervisorId);

            return await query.ToArrayAsync();
        }
        public async Task<Supervisor> GetSupervisorByIdAsync(int employeeId)
        {
            _logger.LogInformation($"Getting the supervisor for ID: {employeeId}.");

            IQueryable<Supervisor> query = _context.Supervisors
                .Include(s => s.Park)
                .Where(s => s.SupervisorId == employeeId);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Supervisor> GetSupervisorByParkIdAsync(int parkId)
        {
            _logger.LogInformation($"Getting the supervisor for park ID: {parkId}.");

            IQueryable<Supervisor> query = _context.Supervisors
                .Include(s => s.Park)
                .Where(s => s.Park.ParkId == parkId);

            return await query.FirstOrDefaultAsync();
        }


        public async Task<ParkEvent[]> GetAllEventsAsync(bool includeChildren = true)
        {
            _logger.LogInformation($"Getting all events.");

            IQueryable<ParkEvent> query;

            if (includeChildren)
            {
                query = _context.Events
                    .Include(c => c.Park)
                    .Include(e => e.User)
                    .Where(e => e.Date >= DateTime.Today)
                .OrderByDescending(e => e.Date);
            }
            else
            {
                query = _context.Events
                    .Where(e => e.Date >= DateTime.Today)
                .OrderByDescending(e => e.Date);
            }

            return await query.ToArrayAsync();
        }
        public async Task<ParkEvent[]> GetEventsByParkIdAsync(int parkId)
        {
            _logger.LogInformation($"Getting events from park ID: {parkId}.");

            IQueryable<ParkEvent> query = _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .Where(e =>
                    e.Park.ParkId == parkId &&
                    e.Date >= DateTime.Today)
                .OrderByDescending(e => e.Date);

            return await query.ToArrayAsync();
        }
        public async Task<ParkEvent> GetEventByIdAsync(int eventId)
        {
            _logger.LogInformation($"Getting events from park ID: {eventId}.");

            IQueryable<ParkEvent> query = _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .Where(e =>
            e.EventId == eventId &&
            e.Date >= DateTime.Today)
                .OrderByDescending(e => e.Date);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<ParkEvent> GetEventByParkIdAsync(int parkId, int eventId)
        {
            _logger.LogInformation($"Getting an event with ID: {eventId} from park ID: {parkId}.");

            IQueryable<ParkEvent> query = _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .Where(e =>
            e.EventId == eventId &&
            e.Park.ParkId == parkId &&
            e.Date >= DateTime.Today);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<ParkEvent> GetEventByParkIdByDateAsync(int parkId, DateTime dateTime)
        {
            _logger.LogInformation(
                $"Getting an event from park ID: {parkId} on date: {dateTime.ToShortDateString()}.");

            IQueryable<ParkEvent> query = _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .Where(e =>
            e.Date == dateTime &&
            e.Park.ParkId == parkId &&
            e.Date >= DateTime.Today);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<ParkEvent[]> GetEventsByDateAsync(DateTime dateTime)
        {
            _logger.LogInformation($"Getting all events.");

            IQueryable<ParkEvent> query = _context.Events
                .Include(e => e.Park)
                .Include(e => e.User)
                .Where(e =>
            e.Date == dateTime &&
            e.Date >= DateTime.Today)
                .OrderByDescending(e => e.Name);

            return await query.ToArrayAsync();
        }
        public async Task<ParkEvent[]> GetAllPastEventsAsync()
        {
            _logger.LogInformation($"Getting all events.");

            IQueryable<ParkEvent> query = _context.Events
                .Include(e => e.Park)
                .Include(e => e.User);

            query = query.OrderByDescending(e => e.Date);

            return await query.ToArrayAsync();
        }

        public async Task<LocalParksUser> GetLocalParksUserByUsernameAsync(string username)
        {
            _logger.LogInformation($"Getting user by username");

            // no support for recursive queries - parks need to be included in services
            IQueryable<LocalParksUser> query = _context.Users
                .Include(u => u.OrganisedEvents)
                .ThenInclude(e => e.Park)
                .Where(u => u.UserName == username);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<LocalParksUser> GetLocalParksUserByEmailAsync(string email)
        {
            _logger.LogInformation($"Getting user by username");

            IQueryable<LocalParksUser> query = _context.Users
                .Include(u => u.OrganisedEvents)
                .ThenInclude(e => e.Park)
                .Where(u => u.Email == email);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product[]> GetAllProductsAsync()
        {
            _logger.LogInformation($"Getting all products.");

            IQueryable<Product> query = _context.Products;

            query = query.OrderByDescending(p => p.ProductId);

            return await query.ToArrayAsync();
        }
        public async Task<Product[]> GetShopProductsAsync()
        {
            _logger.LogInformation($"Getting shop products.");

            IQueryable<Product> query = _context.Products
                .Where(p => p.Category != ProductCategoryType.ClubMembership);

            query = query.OrderByDescending(p => p.ProductId);

            return await query.ToArrayAsync();
        }
        public async Task<Product[]> GetMembershipProductsAsync()
        {
            _logger.LogInformation($"Getting all products.");

            IQueryable<Product> query = _context.Products
                .Where(p => p.Category == ProductCategoryType.ClubMembership);

            query = query.OrderByDescending(p => p.ProductId);

            return await query.ToArrayAsync();
        }
        public async Task<Order[]> GetAllOrdersAsync()
        {
            _logger.LogInformation($"Getting all order.");

            IQueryable<Order> query = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product);

            query = query.OrderByDescending(o => o.DateCreated);

            return await query.ToArrayAsync();
        }
        public async Task<Order[]> GetOrdersByUsernameAsync(string username)
        {
            _logger.LogInformation($"Getting all order of user: {username}.");

            IQueryable<Order> query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User != null && o.User.UserName == username);

            query = query.OrderByDescending(e => e.DateCreated);

            return await query.ToArrayAsync();
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            _logger.LogInformation($"Getting order with Id: {id}.");

            IQueryable<Order> query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.OrderId == id);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Order> GetOrderByNumberAsync(string number)
        {
            _logger.LogInformation($"Getting order with Number: {number}.");

            IQueryable<Order> query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.OrderNumber == number);

            return await query.FirstOrDefaultAsync();
        }
    }
}
