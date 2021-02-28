using LocalParks.Models.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LocalParks.Models
{
    public class ParkEventModel
    {
        [Required]
        public int ParkId { get; set; }
        [IsSortable]
        [DisplayName("Park")]
        public string ParkName { get; set; }
        public int EventId { get; set; }
        [IsSortable]
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Name { get; set; }
        [IsSortable]
        [Required]
        [DateWithoutTime]
        [DateInFuture]
        public DateTime Date { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 30)]
        public string Description { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [DisplayName("Organiser Name")]
        public string OrganiserFirstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [DisplayName("Surname")]
        public string OrganiserLastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "For API POST 'user/this/me' - change to 'user/this/me@email'")]
        [DisplayName("Email address")]
        public string OrganiserEmail { get; set; }
        [Required]
        [Phone]
        [DisplayName("Contact number")]
        public string OrganiserPhoneNumber { get; set; }
        public string Username { get; set; }
        public string ObsfucatedNumber()
        {
            int len = OrganiserPhoneNumber.Length;
            return new StringBuilder(new string('*', len - 3), len).
                Append(OrganiserPhoneNumber[(len - 3)..]).ToString();
        }
    }
}
