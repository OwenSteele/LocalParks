using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
