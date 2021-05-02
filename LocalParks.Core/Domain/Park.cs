using System;
using System.Collections.Generic;

namespace LocalParks.Core.Domain
{
    public class Park
    {
        public int ParkId { get; set; }
        public string PostcodeZone { get; set; }
        public Postcode Postcode { get; set; }
        public string Name { get; set; }
        public int SizeInMetresSquared { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime ClosingTime { get; set; }
        public Supervisor Supervisor { get; set; }
        public ICollection<SportsClub> SportClubs { get; set; }
        public ICollection<ParkEvent> Events { get; set; }
    }
}
