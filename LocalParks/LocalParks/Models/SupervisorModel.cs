using LocalParks.Core;
using LocalParks.Models.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class SupervisorModel
    {
        public Park Park { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; }
        [DateInPast]
        [DateWithoutTime]
        public DateTime StartDate { get; set; }
    }
}
