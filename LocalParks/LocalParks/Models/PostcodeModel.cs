using LocalParks.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models
{
    public class PostcodeModel
    {
        [Required]
        public string Zone { get; set; }
        [Required]
        public string Neighbourhood { get; set; }
        [Required]
        public Int64 Population { get; set; }
        public ICollection<ParkModel> Parks { get; set; }
    }
}