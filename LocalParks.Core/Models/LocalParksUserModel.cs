using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LocalParks.Core.Models
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
        [DisplayName("Contact Number")]
        public string PhoneNumber { get; set; }

        // LocalParks
        [DisplayName("Name")]
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FirstName { get; set; }
        [DisplayName("Surname")]
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string LastName { get; set; }
        [DisplayName("Your Events")]
        public ICollection<ParkEventModel> OrganisedEvents { get; set; }
        [DisplayName("Your Postcode area")]
        [Required]
        public string PostcodeZone { get; set; }
        [DisplayName("Local Parks Member Since")]
        public DateTime MemberSince { get; set; }
    }
}
