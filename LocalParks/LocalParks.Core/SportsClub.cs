namespace LocalParks.Core
{
    public class SportsClub
    {
        public int ClubId { get; set; }
        public string Name { get; set; }
        public Park Park { get; set; }
        public SportType Sport { get; set; }
        public double MembershipFee { get; set; }
        public int Members { get; set; }
    }
}