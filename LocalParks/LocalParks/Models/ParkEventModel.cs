using LocalParks.Core;
using LocalParks.Models.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LocalParks.Models
{
    public class ParkEventModel
    {
        public int ParkId { get; set; }
        public string ParkName { get; set; }

        public int EventId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Name { get; set; }
        [Required]
        [DateWithoutTime]
        [DateInFuture]
        public DateTime Date { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 30)]
        public string Description { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string OrganiserFirstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string OrganiserLastName { get; set; }
        [Required]
        [EmailAddress]
        public string OrganiserEmail { get; set; }
        [Required]
        [Phone]
        public string OrganiserPhoneNumber { get; set; }

        public string ObsfucatedNumber()
        {
            int len = OrganiserPhoneNumber.Length;
            var stars = new string('*', len - 3);
            var obs = new StringBuilder(stars, len);
            obs.Append(OrganiserPhoneNumber[(len - 3)..]);

            return obs.ToString();
        }
    }
}
