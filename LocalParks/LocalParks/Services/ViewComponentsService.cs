﻿using LocalParks.Core.Chart;
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

            var builder = new ChartBuilder(ChartType.line)
                .AddDataX(results.Select(r =>
                    ((int)(DateTime.Now - r.StartingDate).TotalDays / 365.25).ToString()).ToArray())
                .AddDatasetY(
                    results.Select(r => (int)r.Salary).ToArray(),
                    "Salary vs Tenure",
                    "rgba(255, 99, 132, 0.2)",
                    "rgba(255, 99, 132, 1)"
                )
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
                    results.Select(r => r.Parks.Count).ToArray(),
                    "Parks per Postcode"
                )
                .AddBackgroundColors(
                "rgba(255, 99, 132, 0.2)",
                "rgba(54, 162, 235, 0.2)",
                "rgba(255, 206, 86, 0.2)",
                "rgba(75, 192, 192, 0.2)",
                "rgba(153, 102, 255, 0.2)",
                "rgba(255, 159, 64, 0.2)"
                )
                .AddBorderColors(
                "rgba(255, 99, 132, 1)",
                "rgba(54, 162, 235, 1)",
                "rgba(255, 206, 86, 1)",
                "rgba(75, 192, 192, 1)",
                "rgba(153, 102, 255, 1)",
                "rgba(255, 159, 64, 1)"
                )
                .SetTitle("Amount of Parks registered in each Postcode")
                .SetDuration(5);

            var chart = builder.GetChart();

            return new ChartModel(chart);
        }
    }
}