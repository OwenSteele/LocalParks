using LocalParks.Core.Models.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Core.Models
{
    public class SupervisorModel
    {
        public int EmployeeId { get; set; }

        [Required]
        public int ParkId { get; set; }
        public string ParkPostcode { get; set; }
        [IsSortable]
        [DisplayName("Park")]
        public string ParkName { get; set; }
        [IsSortable]
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [DisplayName("Forename")]
        public string FirstName { get; set; }
        [IsSortable]
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [DisplayName("Surname")]
        public string LastName { get; set; }
        [Required]
        [IsSortable]
        [DateInPast]
        [DateWithoutTime]
        [DisplayName("Tenure")]
        public DateTime StartingDate { get; set; }
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email
        {
            get =>$"supervisor.{ParkName.Replace(' ', '_')}@ParkAuthority.co.uk";
        }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [DisplayName("Office Address")]
        public string Office { get; set; }
        [Required]
        [Phone]
        [DisplayName("Emergency Number")]
        public string EmergencyNumber { get; set; }
        [Required]
        public decimal Salary { get; set; }

    }
}
