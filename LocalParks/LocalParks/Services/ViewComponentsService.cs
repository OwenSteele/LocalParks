using LocalParks.Core;
using LocalParks.Core.Chart;
using LocalParks.Data;
using LocalParks.Models.Chart;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class ViewComponentsService : IViewComponentsService
    {
        private readonly IParkRepository _parkRepository;

        public ViewComponentsService(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }

        public async Task<ChartModel> CreateSupervisor_Salary_Tenure_ChartAsync()
        {
            var data = await _parkRepository.GetAllSupervisorsAsync();

            var results = data.OrderBy(d => d.StartingDate).ToArray();

            var builder = new ChartBuilder(ChartType.scatter)
                .AddScatterData(
                results.Select(r => (decimal)((DateTime.Now - r.StartingDate).TotalDays / 365.25)).ToArray(),
                    results.Select(r => r.Salary).ToArray(), 7, 2, 0,
                    "Salary vs Tenure",
                    "rgba(255, 99, 132, 0.2)",
                    "rgba(255, 99, 132, 1)")
                .AddXAxesSet("Service Length")
                .AddYAxesSet("Salary (£)")
                .SetTitle("Salary against Staff Service length")
                .SetDuration(2);

            var chart = builder.GetChart();

            return new ChartModel(chart);
        }
        public async Task<ChartModel> CreateParks_Per_Postcode_ChartAsync()
        {
            var results = await _parkRepository.GetAllPostcodesAsync();

            var builder = new ChartBuilder(ChartType.bar)
                .AddDataX(results.Select(r => r.Zone).ToArray())
                .AddDatasetY(
                    results.Select(r => (decimal)r.Parks.Count).ToArray(),
                    label: "Parks per Postcode",
                    dp: 0
                )
                .AddBackgroundColors(
                "rgba(255, 99, 132, 0.5)",
                "rgba(54, 162, 235, 0.5)",
                "rgba(255, 206, 86, 0.5)",
                "rgba(75, 192, 192, 0.5)",
                "rgba(153, 102, 255, 0.5)",
                "rgba(255, 159, 64, 0.5)"
                )
                .AddBorderColors(
                "rgba(255, 99, 132, 1)",
                "rgba(54, 162, 235, 1)",
                "rgba(255, 206, 86, 1)",
                "rgba(75, 192, 192, 1)",
                "rgba(153, 102, 255, 1)",
                "rgba(255, 159, 64, 1)"
                )
                .AddXAxesSet("Postcode")
                .AddYAxesSet("Number of Parks")
                .SetTitle("Amount of Parks registered in each Postcode")
                .SetDuration(5);

            var chart = builder.GetChart();

            return new ChartModel(chart);
        }
        public async Task<ChartModel> CreateSports_Percentage_ChartAsync()
        {
            var results = await _parkRepository.GetAllSportsClubsAsync();

            var sports = Enum.GetNames<SportType>();

            var count = new decimal[sports.Length];

            foreach (var result in results)
            {
                count[Array.FindIndex(sports, s => s.Equals(result.Sport.ToString()))]++;
            }

            var builder = new ChartBuilder(ChartType.doughnut)
                .AddDataX(sports)
                .AddDatasetY(count,
                    label: "Make up of sports offered by our clubs",
                    borderWidth: 1,
                    dp: 0
                )
                .AddBackgroundColors()
                .SetTitle("Sports of all of our clubs")
                .SetDuration(5);

            var chart = builder.GetChart();

            return new ChartModel(chart);
        }

        public async Task<ChartModel> CreateParkArea_Postcode_ChartAsync()
        {
            var results = await _parkRepository.GetAllPostcodesAsync();

            var builder = new ChartBuilder(ChartType.bar)
                .AddDataX(results.Select(r => r.Zone).ToArray())
                .AddDatasetY(
                    results.Select(r => r.Parks.Sum(p => (decimal)p.SizeInMetresSquared)).ToArray(), 5, 0,
                    "Total park area",
                    borderWidth: 3
                )
                .AddBackgroundColors()
                .AddBorderColors()
                .AddXAxesSet("Postcode")
                .AddYAxesSet("Total area (metres squared)")
                .SetTitle("Overall park area per postcode")
                .SetDuration(2);

            var chart = builder.GetChart();

            return new ChartModel(chart);
        }

        public async Task<ChartModel> CreateClubMembers_Per_ParkSize_ChartAsync()
        {
            var data = await _parkRepository.GetAllSportsClubsAsync();

            var parks = await _parkRepository.GetAllParksAsync(false);

            var count = new decimal[parks.Length];

            foreach (var d in data)
            {
                count[Array.FindIndex(parks, p => p.ParkId == d.Park.ParkId)] += d.Members;
            }

            var builder = new ChartBuilder(ChartType.scatter)
                .AddScatterData(parks.Select(p => (decimal)p.SizeInMetresSquared).ToArray(),
                count,
                7,0,0,
                "Members by park area"
                )
                .AddBackgroundColors()
                .AddBorderColors()
                .AddXAxesSet("Park size (metres squared)")
                .AddYAxesSet("Total club members")
                .SetTitle("Total Club Members by Park area")
                .SetDuration(2);

            var chart = builder.GetChart();

            return new ChartModel(chart);
        }

        public Task<ChartModel> CreateEvents_Per_ParkSize_ChartAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ChartModel> CreateEvents_Per_Month_ChartAsync()
        {
            throw new NotImplementedException();
        }
    }
}
