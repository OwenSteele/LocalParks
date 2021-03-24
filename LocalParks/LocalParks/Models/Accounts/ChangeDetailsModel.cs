using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class ChangeDetailsModel
    {
        [Phone]
        [DisplayName("Contact Number")]
        public string PhoneNumber { get; set; }
        [StringLength(20, MinimumLength = 3)]
        [DisplayName("Forename")]
        public string FirstName { get; set; }
        [StringLength(20, MinimumLength = 3)]
        [DisplayName("Surname")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Your new postcode")]
        public string PostcodeZone { get; set; }
    }
}
