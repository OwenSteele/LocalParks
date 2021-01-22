using LocalParks.Core;
using LocalParks.Models.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class EventModel
    {
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Name { get; set; }
        [Required]
        public Park Park { get; set; }
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
    }
}
