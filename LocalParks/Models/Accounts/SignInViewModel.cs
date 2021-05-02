using LocalParks.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models.Accounts
{
    public class SignInViewModel : LocalParksUserModel
    {
        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
