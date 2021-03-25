using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using LocalParks.Services.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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

        public async Task<HomeModel> GetHomeModelAsync(string latitude, string longitude)
        {
            var parks = _mapper.Map<ParkModel[]>(await _parkRepository.GetAllParksAsync());
            var park = GetSelectedPark(latitude, longitude, parks);

            var parkCount = parks.Length;
            var openParkCount = parks.Where(p =>
             p.ClosingTime.TimeOfDay > DateTime.Now.TimeOfDay && 
             p.OpeningTime.TimeOfDay < DateTime.Now.TimeOfDay).Count();

            var parksClosingSoon = parks.Where(p =>
             p.ClosingTime >= DateTime.Now &&
             p.ClosingTime.Hour <= DateTime.Now.Hour + 2).ToArray();

            var lastevent = _mapper.Map<ParkEventModel>(await _parkRepository.GetLatestEventAsync());
            var upcomingEvents = _mapper.Map<ParkEventModel[]>(await _parkRepository.GetEventsUpToDateAsync(DateTime.Today.AddDays(30)));

            var lastSportsClub = _mapper.Map<SportsClubModel>(await _parkRepository.GetLatestSportsClubAsync());

            return new HomeModel(park,
                                 parkCount,
                                 openParkCount,
                                 parksClosingSoon,
                                 lastevent,
                                 upcomingEvents,
                                 lastSportsClub);
        }


        public async Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync()
        {
            var postcodes = _mapper.Map<IEnumerable<PostcodeModel>>(await _parkRepository.GetAllPostcodesAsync());

            return from p in postcodes
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = p.Zone,
                       Value = p.Zone
                   };
        }

        public bool PostFeedBackAsync(ContactModel model)
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

            if (!latNull.Equals(0) && !longNull.Equals(0))
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
