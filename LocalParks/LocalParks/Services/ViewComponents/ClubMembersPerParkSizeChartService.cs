using LocalParks.Core.Chart;
using LocalParks.Data;
using LocalParks.Models.Chart;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services.ViewComponents
{
    public class ClubMembersPerParkSizeChartService : IChartService
    {

        private readonly IParkRepository _parkRepository;

        public ClubMembersPerParkSizeChartService(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }
        public async Task<ChartModel> CreateAsync()
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
                7, 0, 0,
                "Members by park area"
                )
                .AddBackgroundColors()
                .AddBorderColors()
                .AddXAxesSet("Park size (metres squared)")
                .AddYAxesSet("Total club members")
                .SetTitle("Total Club Members by Park area")
                .SetDuration(2)
                .SetMaintainAspectRatio(true, true);

            var chart = builder.GetChart();

            return new ChartModel(chart);
        }
    }
}
