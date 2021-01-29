using System;
using System.Collections.Generic;

namespace LocalParks.Core
{
    public class Park
    {
        public int ParkId { get; set; }
        public string Postcode { get; set; }
        public string Name { get; set; }
        public int SizeInMetresSquared { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime ClosingTime { get; set; }
        public Supervisor Supervisor { get; set; }
        public ICollection<SportsClub> SportClubs { get; set; }
        public ICollection<ParkEvent> Events { get; set; }
    }
}
