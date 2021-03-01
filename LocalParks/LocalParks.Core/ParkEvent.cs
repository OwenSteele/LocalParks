using System;

namespace LocalParks.Core
{
    public class ParkEvent
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public int ParkId { get; set; }
        public Park Park { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string OrganiserFirstName { get; set; }
        public string OrganiserLastName { get; set; }
        public string OrganiserEmail { get; set; }
        public string OrganiserPhoneNumber { get; set; }
        public LocalParksUser User { get; set; }
        public string Recurring { get; set; }
    }
}
