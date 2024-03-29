﻿namespace LocalParks.Core.Domain
{
    public class SportsClub
    {
        public int ClubId { get; set; }
        public string Name { get; set; }
        public int ParkId { get; set; }
        public Park Park { get; set; }
        public SportType Sport { get; set; }
        public decimal MembershipFee { get; set; }
        public int Members { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string President { get; set; }
    }
}