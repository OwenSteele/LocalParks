using LocalParks.Services;
using LocalParks.Services.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.ViewComponents
{
    public class PopularShopProductsChartViewComponent : ViewComponent
    {
        private readonly IChartService _service;

        public PopularShopProductsChartViewComponent(ChartServiceResolver resolver)
        {
            _service = resolver(ChartServiceType.PopularShopProducts);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _service.CreateAsync();

            return View(model);
        }
    }
}
