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
    public class PopularShopProductsChartService : IChartService
    {
        private readonly IParkRepository _parkRepository;

        public PopularShopProductsChartService(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }
        public async Task<ChartModel> CreateAsync()
        {
            var results = await _parkRepository.GetAllOrdersAsync();

            var products = await _parkRepository.GetShopProductsAsync();

            var count = new decimal[products.Length];

            var itemsPurchased = results.Sum(r => r.Items.Sum(i => i.Quantity));

            foreach (var order in results)
                foreach (var item in order.Items)
                {
                    count[Array.FindIndex(products, p => p.ProductId == item.ProductId)] += item.Quantity;
                }

            for (int i = 0; i < products.Length; i++)
            {
                count[i] = (count[i] / itemsPurchased) * 100m;
            }

            var builder = new ChartBuilder(ChartType.pie)
                .AddDataX(products.Select(p => p.Name).ToArray())
                .AddDatasetY(count,
                    dp: 2,
                    label: "% of all purchased items"
                )
                .AddBackgroundColors()
                .SetTitle("Percentage of items purchased from all shop orders")
                .SetDuration(2)
                .SetMaintainAspectRatio(true, true);

            var chart = builder.GetChart();

            return new ChartModel(chart);
        }
    }
}
