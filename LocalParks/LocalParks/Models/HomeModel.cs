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
        public int OpenParksCount { get; set; }

        public ICollection<ParkModel> ParksClosingSoon { get; set; }
        public ICollection<ParkEventModel> EventsThisMonth { get; set; }
        public ParkModel NearestPark { get; set; }
        public string ClosestEmergencyNumber { get; set; }

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

            OpenParksCount = parks.Where(p =>
            p.ClosingTime.TimeOfDay > DateTime.Now.TimeOfDay && p.OpeningTime.TimeOfDay < DateTime.Now.TimeOfDay).Count();

            ParksClosingSoon = Parks.Where(p =>
            p.ClosingTime.Hour >= DateTime.Now.Hour &&
            p.ClosingTime.Hour <= DateTime.Now.Hour + 2).ToArray();

            EventsThisMonth = allEvents.Where(e => e.Date.DayOfYear >= DateTime.Now.DayOfYear && e.Date.DayOfYear <= DateTime.Now.DayOfYear + 30).ToArray();

            if (latitude != null && longitude != null)
            {
                var nearest = parks.OrderBy(p =>
                 Math.Pow((double)p.Latitude - (double)latitude, 2) +
                 Math.Pow((double)p.Longitude - (double)longitude, 2)).First();

                if (nearest.Supervisor == null)
                    nearest.Supervisor = new SupervisorModel { EmergencyNumber = "Head Office: 01189991234" };

                NearestPark = nearest;

                ClosestEmergencyNumber = nearest.Supervisor.EmergencyNumber;
            }
            else
            {
                var rnd = new Random();
                var rndPark = Parks.ElementAt(rnd.Next(0, Parks.Count - 1));

                if (rndPark.Supervisor == null)
                    rndPark.Supervisor = new SupervisorModel { EmergencyNumber = "Head Office: 01189991234" };

                NearestPark = rndPark;
            }
        }
    }
}
