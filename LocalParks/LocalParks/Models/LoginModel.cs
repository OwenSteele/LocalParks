using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 8)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
