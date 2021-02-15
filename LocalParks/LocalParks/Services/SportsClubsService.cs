using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class SportsClubsService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        public SportsClubsService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<SportsClubModel[]> GetAllSportsClubModelsAsync()
        {
            var results = await _parkRepository.GetAllSportsClubsAsync();

            return _mapper.Map<SportsClubModel[]>(results);
        }
        public async Task<SportsClubModel[]> GetSearchedSportsClubModelsAsync(string searchTerm)
        {
            var results = await _parkRepository.GetAllSportsClubsAsync();

            searchTerm = searchTerm.ToLower();

            var matches = results.Where(p =>
                p.Name.ToLower() == searchTerm |
                p.Name.ToLower().Contains(searchTerm) |
                p.Name.ToLower().StartsWith(searchTerm))
                    .ToArray();

            if (!matches.Any()) return null;

            return _mapper.Map<SportsClubModel[]>(matches);
        }
        public async Task<SportsClubModel> GetSportsClubModelAsync(int clubId, int? parkId = null)
        {
            var result = await _parkRepository.GetSportsClubByIdAsync(clubId, parkId);

            if (result == null) return null;

            return _mapper.Map<SportsClubModel>(result);
        }
        public async Task<SportsClubModel[]> GetSportsClubModelsByParkAsync(int parkId)
        {
            var results = await _parkRepository.GetSportsClubsByParkIdAsync(parkId);

            if (!results.Any()) return null;

            return _mapper.Map<SportsClubModel[]>(results);
        }
        public async Task<SportsClubModel[]> GetSportsClubModelsBySportAsync(int parkId, SportType sportType)
        {
            var results = await _parkRepository.GetSportsClubsBySportAsync(sportType, parkId);

            if (!results.Any()) return null;

            return _mapper.Map<SportsClubModel[]>(results);
        }
    }
}
