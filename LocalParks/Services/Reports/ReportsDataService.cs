using LocalParks.Core.Domain;
using LocalParks.Core.Domain.Shop;
using LocalParks.Data;
using System;
using System.Threading.Tasks;

namespace LocalParks.Services.Reports
{
    public class ReportsDataService : IReportsDataService
    {
        private readonly IParkRepository _parkRepository;

        public ReportsDataService(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }

        public async Task<Park[]> GetAllParks()
        {
            return await _parkRepository.GetAllParksAsync();
        }

        public async Task<Supervisor[]> GetAllSupervisors()
        {
            return await _parkRepository.GetAllSupervisorsAsync();
        }

        public async Task<SportsClub[]> GetAllSportsClubs()
        {
            return await _parkRepository.GetAllSportsClubsAsync();
        }

        public async Task<ParkEvent[]> GetAllParkEvents()
        {
            return await _parkRepository.GetAllEventsAsync();
        }

        public async Task<Order[]> GetAllOrders()
        {
            return await _parkRepository.GetAllOrdersAsync();
        }
        public async Task<Product[]> GetAllProducts()
        {
            return await _parkRepository.GetAllProductsAsync();
        }

        public int SportCount() => Enum.GetNames(typeof(SportType)).Length;
        public string SportName(int pos) => ((SportType)pos).ToString();
    }
}
