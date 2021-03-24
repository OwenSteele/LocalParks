using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class SignInModel : LocalParksUserModel
    {
        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
