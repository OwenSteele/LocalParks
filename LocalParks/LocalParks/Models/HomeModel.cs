using System;
using System.Collections.Generic;
using System.Linq;

namespace LocalParks.Models
{
    public class HomeModel
    {
        public ICollection<ParkModel> Parks { get; set; }
        public ICollection<PostcodeModel> Postcodes { get; set; }
        public SportsClubModel NewestSportsClub { get; set; }
        public ParkEventModel RecentEvent { get; private set; }
        public ICollection<ParkEventModel> AllEvents { get; set; }
        public ParkModel NearestPark { get; set; }
        public SupervisorModel NearestSupervisor { get; set; }
        public int OpenParksCount { get; set; }
        public ICollection<ParkEventModel> EventsThisMonth { get; set; }

        public HomeModel(ICollection<ParkModel> parks,
            ICollection<PostcodeModel> postcodes,
            ICollection<ParkEventModel> allEvents,
            SportsClubModel sportsClub, double? latitude = null, double? longitude = null)
        {
            Parks = parks;
            Postcodes = postcodes;
            AllEvents = allEvents;
            NewestSportsClub = sportsClub;
            RecentEvent = allEvents.Last();

            if (latitude == null || longitude == null)
            {
                latitude = 40;
                longitude = 0;
            }

            NearestPark = parks.OrderBy(p =>
            Math.Pow((double)p.Latitude - (double)latitude, 2) +
            Math.Pow((double)p.Longitude - (double)longitude, 2)).First();

            NearestSupervisor = NearestPark.Supervisor;

            OpenParksCount = parks.Where(p => 
            p.ClosingTime.TimeOfDay > DateTime.Now.TimeOfDay).Count();

            EventsThisMonth = allEvents.Where(e => e.Date.Month == DateTime.Now.Month).ToArray();
        }
    }
}
