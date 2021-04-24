using LocalParks.Core.Chart;
using LocalParks.Data;
using LocalParks.Infrastructure.Builders;
using LocalParks.Models.Chart;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace LocalParks.Services.ViewComponents
{
    public class EventsPerMonthChartService : IChartService
    {
        private readonly IParkRepository _parkRepository;

        public EventsPerMonthChartService(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }

        public async Task<ChartViewModel> CreateAsync()
        {
            var results = await _parkRepository.GetAllEventsAsync();

            var count = new decimal[12];

            var months = new string[12];

            for (int i = 0; i < 12; i++) months[i] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1);

            Console.Write(results);

            foreach (var r in results) count[r.Date.Month - 1]++;

            var builder = new ChartBuilder(ChartType.line)
                .AddDataX(months)
                .AddDatasetY(count,
                    label: "Number of Events",
                    borderWidth: 1
                )
                .AddBackgroundColors()
                .AddBorderColors()
                .AddXAxesSet("Month")
                .AddYAxesSet("Events count")
                .SetTitle("Number of events organised per month")
                .SetDuration(2)
                .SetMaintainAspectRatio(true, true);

            var chart = builder.GetChart();

            return new ChartViewModel(chart);
        }
    }
}
