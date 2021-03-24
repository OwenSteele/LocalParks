using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.ViewComponents
{
    public class PopularShopProductsChartViewComponent : ViewComponent
    {
        private readonly IViewComponentsService _service;

        public PopularShopProductsChartViewComponent(IViewComponentsService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _service.CreatePopular_Shop_Products_ChartAsync();

            return View(model);
        }
    }
}
