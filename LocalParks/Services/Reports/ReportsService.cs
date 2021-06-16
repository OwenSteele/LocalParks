using LocalParks.Core.Domain.Shop;
using LocalParks.Models.Reports;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services.Reports
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsDataService _service;
        private readonly ReportsViewModel _model;

        public ReportsService(IReportsDataService service)
        {
            _service = service;
            _model = new();
        }

        public async Task<ReportsViewModel> GetReportsModelAsync()
        {
            await SetParksData();
            await SetSportsClubsData();
            await SetEventsData();
            await SetSupervisorsData();
            await SetShopData();

            return _model;
        }
        private async Task SetParksData()
        {
            var results = await _service.GetAllParks();

            _model.ParkCount = results.Length;
            _model.TotalParkArea = results.Sum(p => p.SizeInMetresSquared);
            _model.AverageClubsPerPark = Math.Round(results.Average(p => p.SportClubs.Count), 2);
            _model.MinClubsPerPark = results.Min(p => p.SportClubs.Count);
            _model.MaxClubsPerPark = results.Max(parks => parks.SportClubs.Count);
            _model.AverageEventsPerPark = Math.Round(results.Average(p => p.Events.Count), 2);
            _model.MinEventsPerPark = results.Min(p => p.Events.Count);
            _model.MaxEventsPerPark = results.Max(parks => parks.Events.Count);
        }
        private async Task SetSportsClubsData()
        {
            var results = await _service.GetAllSportsClubs();

            _model.ClubCount = results.Length;
            _model.AverageClubMembers = (int)Math.Round(results.Average(c => c.Members), 0);
            var max = results.Max(c => c.Members);
            _model.LargestClub = (max, results.Where(c => c.Members == max).FirstOrDefault().Name);
            _model.AverageClubMembership = Math.Round(results.Average(c => c.MembershipFee), 2);

            var sports = new int[await _service.SportCount()];

            foreach (var result in results)
            {
                sports[(int)result.Sport.SportTypeId]++;
            }

            _model.MostPopularSport = await _service.SportName(sports.Max());
        }
        private async Task SetEventsData()
        {
            var results = await _service.GetAllParkEvents();
            _model.EventCount = results.Length;

            var months = new int[13];
            var days = new int[7];

            foreach (var result in results)
            {
                months[result.Date.Month - 1]++;
                days[((int)result.Date.DayOfWeek)]++;
            }

            _model.MostPopularMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(months.Max() + 1);
            _model.LeastPopularMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(months.Min() + 1);
            _model.MostPopularDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)days.Max());
            _model.LeastPopularDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)days.Min());
        }
        private async Task SetSupervisorsData()
        {
            var results = await _service.GetAllSupervisors();

            var tenures = results.Select(s => (DateTime.Now - s.StartingDate).TotalDays).ToArray();

            var average = TimeSpan.FromDays(tenures.Average());

            _model.AverageTenure = Math.Round(average.TotalDays / 365.25, 0).ToString();

            _model.AverageSalary = Math.Round(results.Average(s => s.Salary), 2);
            _model.RValueSalaryParkArea = Math.Round(RCalc(results.Select(s =>
            (double)s.Salary).ToArray(),
            results.Select(s => (double)s.Park.SizeInMetresSquared).ToArray()), 6);
            _model.RValueSalaryTenure = Math.Round(RCalc(results.Select(s =>
            (double)s.Salary).ToArray(), tenures), 6);
            _model.LongestTenure = Math.Round(tenures.Max() / 365.25, 0).ToString();
        }

        private async Task SetShopData()
        {
            var results = await _service.GetAllOrders();

            _model.TotalOrders = results.Length;
            _model.TotalItemsOrdered = results.Sum(o => o.Items.Count);
            _model.AverageOrderValue = Math.Round(results.Average(o => OrderTotal(o)), 2);
            _model.AverageOrderItems = Math.Round(results.Average(o => o.Items.Count), 0);
        }
        private static double RCalc(double[] x, double[] y)
        {
            var avg1 = x.Average();
            var avg2 = y.Average();

            var sum1 = x.Zip(y, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();

            var sqr1 = x.Sum(x => Math.Pow((x - avg1), 2.0));
            var sqr2 = y.Sum(y => Math.Pow((y - avg2), 2.0));

            var result = sum1 / Math.Sqrt(sqr1 * sqr2);

            return result;
        }
        private static decimal OrderTotal(Order order) => Math.Round(order.Items.Sum(i => i.UnitPrice * i.Quantity), 2);
    }
}

