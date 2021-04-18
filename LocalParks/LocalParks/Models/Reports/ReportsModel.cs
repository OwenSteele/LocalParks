namespace LocalParks.Models.Reports
{
    public class ReportsModel
    {
        public int ParkCount { get; set; }
        public int TotalParkArea { get; set; }
        public double AverageClubsPerPark { get; set; }
        public int MinClubsPerPark { get; set; }
        public int MaxClubsPerPark { get; set; }
        public double AverageEventsPerPark { get; set; }
        public int MinEventsPerPark { get; set; }
        public int MaxEventsPerPark { get; set; }

        public int ClubCount { get; set; }
        public int AverageClubMembers { get; set; }
        public (int, string) LargestClub { get; set; }
        public decimal AverageClubMembership { get; set; }
        public string MostPopularSport { get; set; }

        public int EventCount { get; set; }
        public string MostPopularMonth { get; set; }
        public string LeastPopularMonth { get; set; }
        public string MostPopularDayOfWeek { get; set; }
        public string LeastPopularDayOfWeek { get; set; }

        public string AverageTenure { get; set; }
        public decimal AverageSalary { get; set; }
        public double RValueSalaryParkArea { get; set; }
        public double RValueSalaryTenure { get; set; }
        public string LongestTenure { get; set; }

        public int TotalOrders { get; set; }
        public int TotalItemsOrdered { get; set; }
        public decimal AverageOrderValue { get; set; }
        public double AverageOrderItems { get; set; }
    }
}
