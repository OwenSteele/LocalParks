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

            var builder = new ChartBuilder();


            builder.AddDataLabels(results.Select(r =>
            ((DateTime.Now - r.StartingDate).TotalDays / 365.25).ToString()).ToArray());

            builder.AddDataset(
                results.Select(r => (int)r.Salary).ToArray(),
                "Salary vs Tenure"
                );

            builder.SetType("line");

            builder.SetTitle("Salary against Staff Service length");

            var chart = builder.GetChart();

            var model = new ChartModel(chart);

            return model;
        }
    }
}
