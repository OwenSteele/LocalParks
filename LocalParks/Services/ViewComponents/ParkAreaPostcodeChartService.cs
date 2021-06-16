using LocalParks.Core.Chart;
using LocalParks.Data;
using LocalParks.Infrastructure.Builders;
using LocalParks.Models.Chart;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services.ViewComponents
{
    public class ParkAreaPostcodeChartService : IChartService
    {
        private readonly IParkRepository _parkRepository;

        public ParkAreaPostcodeChartService(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }
        public async Task<ChartViewModel> CreateAsync()
        {
            var results = await _parkRepository.GetAllPostcodesAsync();

            var builder = new ChartBuilder(ChartType.bar)
                .AddDataX(results.Select(r => r.Zone).ToArray())
                .AddDatasetY(
                    results.Select(r => r.Parks.Sum(p => (decimal)p.SizeInMetresSquared)).ToArray(), 5, 0,
                    "Total park area",
                    borderWidth: 3
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
                .AddYAxesSet("Total area (metres squared)")
                .SetTitle("Overall park area per postcode")
                .SetDuration(2)
                .SetMaintainAspectRatio(true, true);

            var chart = builder.GetChart();

            return new ChartViewModel(chart);
        }
    }
}
