using LocalParks.Core;
using LocalParks.Core.Chart;
using LocalParks.Data;
using LocalParks.Models.Chart;
using System;
using System.Threading.Tasks;

namespace LocalParks.Services.ViewComponents
{
    public class SportsPercentageChartService : IChartService
    {
        private readonly IParkRepository _parkRepository;

        public SportsPercentageChartService(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }
        public async Task<ChartModel> CreateAsync()
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
                .SetDuration(5)
                .SetMaintainAspectRatio(true, true);

            var chart = builder.GetChart();

            return new ChartModel(chart);
        }
    }
}
