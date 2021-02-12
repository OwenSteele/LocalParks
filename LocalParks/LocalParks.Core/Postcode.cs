using System;
using System.Collections.Generic;

namespace LocalParks.Core
{
    public class Postcode
    {
        public string Zone { get; set; }
        public string Neighbourhood { get; set; }
        public Int64 Population { get; set; }
        public ICollection<Park> Parks { get; set; }
    }
}