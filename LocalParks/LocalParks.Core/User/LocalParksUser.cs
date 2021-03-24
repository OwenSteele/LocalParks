using LocalParks.Core.Shop;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace LocalParks.Core
{
    public class LocalParksUser : IdentityUser
    {
        //UserName, Email, PhoneNumber in Base
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<ParkEvent> OrganisedEvents { get; set; }
        public string PostcodeZone { get; set; }
        public DateTime MemberSince { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
