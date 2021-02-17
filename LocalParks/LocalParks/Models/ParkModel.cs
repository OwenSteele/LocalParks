﻿using System;
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
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        [DisplayName("Park Area")]
        public int SizeInMetresSquared { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        [DisplayName("Opens")]
        public DateTime OpeningTime { get; set; }
        [DisplayName("Closes")]
        public DateTime ClosingTime { get; set; }
        public SupervisorModel Supervisor { get; set; }
        [DisplayName("Sports Clubs")]
        public ICollection<SportsClubModel> SportClubs { get; set; }
        public ICollection<ParkEventModel> Events { get; set; }
    }
}
