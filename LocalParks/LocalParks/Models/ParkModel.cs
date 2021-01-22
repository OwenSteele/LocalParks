using LocalParks.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class ParkModel
    {
        [Required]
        public string Postcode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 20)]
        public string Name { get; set; }
        public int SizeInMetresSquared { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime ClosingTime { get; set; }

        public ICollection<SportsClubModel> SportClubs { get; set; }
        [Required]
        public Supervisor Supervisor { get; set; }
    }
}
