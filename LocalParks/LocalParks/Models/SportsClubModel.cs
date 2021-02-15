using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class SportsClubModel
    {
        public int ParkId { get; set; }
        [DisplayName("Located in Park")]
        public string ParkName { get; set; }

        public int ClubId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        public string Sport { get; set; }
        public double MembershipFee { get; set; }
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
