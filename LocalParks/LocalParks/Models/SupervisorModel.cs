using LocalParks.Models.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class SupervisorModel
    {
        public int EmployeeId { get; set; }

        public int ParkId { get; set; }
        public string ParkPostcode { get; set; }
        public string ParkName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; }
        [DateInPast]
        [DateWithoutTime]
        public DateTime StartingDate { get; set; }
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email
        {
            get { return $"supervisor.{ParkName.Replace(' ', '_')}@ParkAuthority.co.uk"; }
        }

        [StringLength(100, MinimumLength = 2)]
        [DisplayName("Office Address")]
        public string Office { get; set; }
        [Phone]
        [DisplayName("Emergency Number")]
        public string EmergencyNumber { get; set; }

        public string Tenure()
        {
            if (StartingDate == DateTime.MinValue) return "N/A";

            var days = Math.Floor((DateTime.Now - StartingDate).TotalDays);
            if (days < 1) return "Less than a day";
            if (days == 1) return "A day";

            var months = Math.Floor(days / (365.25 / 12));
            if (months < 1) return $"{days} days";
            if (months == 1) return "A month";

            var years = Math.Floor(days / 365.25);
            if (years < 1) return $"{months} months";
            if (years == 1) return "A year";

            return $"{years} years";
        }

    }
}
