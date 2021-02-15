using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Models
{
    public class HomeModel
    {
        public ICollection<ParkModel> Parks { get; set; }
        public ICollection<PostcodeModel> Postcodes { get; set; }
        public SportsClubModel NewestSportsClub { get; set; }
        public ParkEventModel RecentEvent { get; private set; }
        public ICollection<ParkEventModel> AllEvents { get; set; }

        public HomeModel(ICollection<ParkModel> parks, 
            ICollection<PostcodeModel> postcodes, 
            ICollection<ParkEventModel> allEvents,
            SportsClubModel sportsClub)
        {
            Parks = parks;
            Postcodes = postcodes;
            AllEvents = allEvents;
            NewestSportsClub = sportsClub;
            RecentEvent = allEvents.Last();
        }

        public ParkModel NearestPark(double longitude, double latitude) =>
            Parks.OrderBy(p => (p.Latitude - (decimal)latitude) * (p.Longitude - (decimal)longitude)).First();

        public SupervisorModel NearestSupervisor(double longitude, double latitude) =>
            Parks.OrderBy(p => (p.Latitude - (decimal)latitude) * (p.Longitude - (decimal)longitude)).First().Supervisor;

        public int OpenParksCount() =>
            Parks.Where(p => p.ClosingTime.TimeOfDay > DateTime.Now.TimeOfDay).Count();

        public ICollection<ParkEventModel> EventsThisMonth() =>
            AllEvents.Where(e => e.Date.Month == DateTime.Now.Month).ToArray();
    }
}
