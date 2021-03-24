using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.ViewComponents
{
    public class TotalParkAreaPerPostcodeChartViewComponent : ViewComponent
    {
        private readonly IViewComponentsService _service;

        public TotalParkAreaPerPostcodeChartViewComponent(IViewComponentsService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _service.CreateParkArea_Postcode_ChartAsync();

            return View(model);
        }

    }
}
