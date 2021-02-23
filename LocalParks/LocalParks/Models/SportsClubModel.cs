﻿using LocalParks.Models.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class SportsClubModel
    {
        [Required]
        public int ParkId { get; set; }
        [IsSortable]
        [DisplayName("Park")]
        public string ParkName { get; set; }
        public int ClubId { get; set; }
        [IsSortable]
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        public string Sport { get; set; }
        [IsSortable]
        public double MembershipFee { get; set; }
        [IsSortable]
        public int Members { get; set; }
        [Url]
        [StringLength(1000, MinimumLength = 5)]
        public string Website { get; set; }
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }
        [StringLength(150, MinimumLength = 5)]
        [DisplayName("Club President")]
        public string President { get; set; }
    }
}
