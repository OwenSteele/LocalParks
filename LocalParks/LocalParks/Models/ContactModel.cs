using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class ContactModel
    {
        [Required]
        [DisplayName("Your name")]
        [StringLength(100, MinimumLength = 8)]
        public string FullName { get; set; }
        [Required]
        [DisplayName("Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [DisplayName("Contact Number")]
        [Phone]
        public string PhoneNumber { get; set; }
        [DisplayName("Your Postcode")]
        [Required]
        public string Postcode { get; set; }
        [Required]
        [DisplayName("Your feedback")]
        [StringLength(1000, MinimumLength = 30)]
        public string Message { get; set; }
    }
}
