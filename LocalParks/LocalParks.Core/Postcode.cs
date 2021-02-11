using System.Collections.Generic;

namespace LocalParks.Core
{
    public class Postcode
    {
        public string PostcodeZone { get; set; }
        public string Neighbourhood { get; set; }
        public long Population { get; set; }
        public ICollection<Park> Parks { get; set; }
    }
}