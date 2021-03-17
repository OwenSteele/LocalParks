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
            var results = await _parkRepository.GetAllSupervisorsAsync();

            var dataset = new Dataset[] {
            new Dataset {
                Label = "Salary vs Tenure",
                Data = results.Select(r => r.Salary).ToArray(),
                XAxisID = "Tenure (Years)",
                YAxisID = "Salary (£)"
            } };


            var data = new Core.Chart.Data
            {
                Labels = results.Select(r => ((DateTime.Now - r.StartingDate).TotalDays / 365.25).ToString()).ToArray(),
                Datasets = dataset
            };


            var chart = new ReportChart(data, type: "line");

            return new ChartModel(chart);
        }
    }
}
