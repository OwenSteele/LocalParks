using LocalParks.Core.Chart;
using LocalParks.Data;
using LocalParks.Models.Chart;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services.ViewComponents
{
    public class ParksPerPostcodeChartService : IChartService
    {

        private readonly IParkRepository _parkRepository;

        public ParksPerPostcodeChartService(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }

        public async Task<ChartModel> CreateAsync()
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
                .SetDuration(5)
                .SetMaintainAspectRatio(true, true);

            var chart = builder.GetChart();

            return new ChartModel(chart);
        }
    }
}
