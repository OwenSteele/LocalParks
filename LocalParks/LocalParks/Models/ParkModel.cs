using LocalParks.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class ParkModel
    {
        public int ParkId { get; set; }
        [IsSortable]
        [Required]
        [DisplayName("Postcode")]
        public string PostcodeZone { get; set; }
        [IsSortable]
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        [IsSortable(false)]
        [DisplayName("Park Area")]
        public int SizeInMetresSquared { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        [IsSortable]
        [DisplayName("Opens")]
        public DateTime OpeningTime { get; set; }
        [IsSortable(false)]
        [DisplayName("Closes")]
        public DateTime ClosingTime { get; set; }
        public SupervisorModel Supervisor { get; set; }
        [DisplayName("Sports Clubs")]
        public ICollection<SportsClubModel> SportClubs { get; set; }
        public ICollection<ParkEventModel> Events { get; set; }
    }
}
