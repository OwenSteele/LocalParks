using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class HomeService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public HomeService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<HomeModel> GetHomeModelAsync(string latitude, string longitude)
        {
            var parks = await _parkRepository.GetAllParksAsync();
            var parkModels = _mapper.Map<ParkModel[]>(parks);

            var postcodes = await _parkRepository.GetAllPostcodesAsync();
            var postcodesModels = _mapper.Map<PostcodeModel[]>(postcodes);

            var events = await _parkRepository.GetAllEventsAsync();
            var eventsModels = _mapper.Map<ParkEventModel[]>(events);

            var sportsClubs = await _parkRepository.GetAllSportsClubsAsync();
            var lastSportsClub = sportsClubs[^1];

            var sportsClubsModel = _mapper.Map<SportsClubModel>(lastSportsClub);

            double? latNull = null;
            double? longNull = null;

            if (double.TryParse(latitude, out double lat) &&
                double.TryParse(longitude, out double lon))
            {
                latNull = lat;
                longNull = lon;
            }

            return new HomeModel(parkModels,
                                postcodesModels,
                                eventsModels,
                                sportsClubsModel, latNull,longNull);
        }
    }
}
