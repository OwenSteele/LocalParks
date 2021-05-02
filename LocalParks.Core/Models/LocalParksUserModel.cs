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
        [DisplayName("Local Parks Member for")]

        public string MembershipLength
        {
            get
            {

                if (MemberSince == DateTime.MinValue) return "N/A";

                var days = Math.Floor((DateTime.Now - MemberSince).TotalDays);
                if (days < 1) return "Less than a day";

                var result = new StringBuilder();

                var years = Math.Floor(days / 365.25);
                if (years > 1) result.Append($"{years} years");
                if (years == 1) result.Append($"{years} year");

                var months = Math.Floor((days - (years * 365.25)) / (365.25 / 12));
                if (months > 1) result.Append($"{months} months");
                if (months == 1) result.Append($"{months} month");

                days = Math.Floor((days - (years * 365.25))) - (months * (365.25 / 12));
                if (days > 1) result.Append($"{days} days");
                if (days == 1) result.Append($"{days} day");

                return result.ToString();
            }
        }
    }
}
