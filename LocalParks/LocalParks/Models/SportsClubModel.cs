using LocalParks.Models.Validation;
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
        [Required]
        public string Sport { get; set; }
        [Required]
        [IsSortable]
        [DisplayName("Annual Membership Fee")]
        public double MembershipFee { get; set; }
        [Required]
        [IsSortable]
        public int Members { get; set; }
        [Required]
        [Url]
        [StringLength(1000, MinimumLength = 5)]
        public string Website { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 5)]
        [DisplayName("Club President")]
        public string President { get; set; }
    }
}
