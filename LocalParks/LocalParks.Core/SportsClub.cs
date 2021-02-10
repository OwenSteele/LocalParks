namespace LocalParks.Core
{
    public class SportsClub
    {
        public int ClubId { get; set; }
        public string Name { get; set; }
        public Park Park { get; set; }
        public SportType Sport { get; set; }
        //money should be decimal as you get founding errors with double/float
        public double MembershipFee { get; set; }
        public int Members { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string President { get; set; }
    }
}
