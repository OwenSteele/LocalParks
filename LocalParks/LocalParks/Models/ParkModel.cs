using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class ParkModel
    {
        public int ParkId { get; set; }
        [Required]
        [DisplayName("Postcode")]
        public string PostcodeZone { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 20)]
        public string Name { get; set; }
        [DisplayName("Park Area")]
        public int SizeInMetresSquared { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [DisplayName("Opens")]
        public DateTime OpeningTime { get; set; }
        [DisplayName("Closes")]
        public DateTime ClosingTime { get; set; }
        [Required]
        public SupervisorModel Supervisor { get; set; }
        [DisplayName("Sports Clubs")]
        public ICollection<SportsClubModel> SportClubs { get; set; }
        public ICollection<ParkEventModel> Events { get; set; }
    }
}
