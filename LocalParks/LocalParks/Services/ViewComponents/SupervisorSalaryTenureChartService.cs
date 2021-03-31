using LocalParks.Core.Chart;
using LocalParks.Data;
using LocalParks.Models.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services.ViewComponents
{
    public class SupervisorSalaryTenureChartService : IChartService
    {

        private readonly IParkRepository _parkRepository;

        public SupervisorSalaryTenureChartService(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }
        public async Task<ChartModel> CreateAsync()
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
                .SetDuration(2)
                .SetMaintainAspectRatio(true, true);

            var chart = builder.GetChart();

            return new ChartModel(chart);
        }
    }
}
