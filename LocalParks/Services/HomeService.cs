using AutoMapper;
using LocalParks.Core.Contracts.Shared;
using LocalParks.Core.Models;
using LocalParks.Data;
using LocalParks.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class HomeService : IHomeService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        private readonly IRandomService _service;

        public HomeService(IParkRepository parkRepository,
            IMapper mapper,
            IRandomService service)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
            _service = service;
        }

        public async Task<HomeViewModel> GetHomeModelAsync(string latitude, string longitude)
        {
            var parks = _mapper.Map<ParkModel[]>(await _parkRepository.GetAllParksAsync());
            var park = GetSelectedPark(latitude, longitude, parks);

            var timeNow = DateTime.Now.AddHours(1).TimeOfDay;

            var parkCount = parks.Length;
            var openParkCount = parks.Where(p =>
             p.ClosingTime.TimeOfDay > timeNow &&
             p.OpeningTime.TimeOfDay < timeNow).Count();

            var parksClosingSoon = parks.Where(p =>
             p.ClosingTime.TimeOfDay > timeNow &&
             (p.ClosingTime.TimeOfDay < DateTime.Now.AddHours(3).TimeOfDay |
              (DateTime.Now.AddHours(1).DayOfYear + 1) == DateTime.Now.AddHours(3).DayOfYear)).ToArray();

            var lastevent = _mapper.Map<ParkEventModel>(await _parkRepository.GetLatestEventAsync());
            var upcomingEvents = _mapper.Map<ParkEventModel[]>(await _parkRepository.GetEventsUpToDateAsync(DateTime.Today.AddDays(30)));

            var lastSportsClub = _mapper.Map<SportsClubModel>(await _parkRepository.GetLatestSportsClubAsync());

            return new HomeViewModel(park,
                                 parkCount,
                                 openParkCount,
                                 parksClosingSoon,
                                 lastevent,
                                 upcomingEvents,
                                 lastSportsClub);
        }

        public bool PostFeedBackAsync(ContactViewModel model)
        {
            //implement DB table with feedback

            return true;
        }

        private ParkModel GetSelectedPark(string latitude, string longitude, ParkModel[] parks)
        {
            double? latNull = null;
            double? longNull = null;

            if (double.TryParse(latitude, out double lat) &&
                double.TryParse(longitude, out double lon))
            {
                latNull = lat;
                longNull = lon;
            }

            if (!latNull.HasValue && !longNull.HasValue)
            {
                var value = _service.Generate(parks.Length - 1);
                return parks.ElementAt(value);
            }

            return parks.OrderBy(p =>
                Math.Pow((double)p.Latitude - (double)latNull, 2) +
                Math.Pow((double)p.Longitude - (double)longNull, 2)).First();
        }
    }
}
