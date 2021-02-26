using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class LocalParksUserModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        // LocalParks
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string LastName { get; set; }
        public ICollection<ParkEventModel> OrganisedEvents { get; set; }
        public string PostcodeZone { get; set; }
    }
}
