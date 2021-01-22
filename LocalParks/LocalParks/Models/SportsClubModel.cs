using LocalParks.Core;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class SportsClubModel
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        public Park ParkId { get; set; }
        public SportType Sport { get; set; }
        public double MembershipFee { get; set; }
        public int Members { get; set; }
    }
}
