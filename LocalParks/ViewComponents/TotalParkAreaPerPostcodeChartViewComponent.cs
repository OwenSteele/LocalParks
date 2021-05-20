using LocalParks.Services.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.ViewComponents
{
    public class TotalParkAreaPerPostcodeChartViewComponent : ViewComponent
    {
        private readonly IChartService _service;

        public TotalParkAreaPerPostcodeChartViewComponent(ChartServiceResolver resolver)
        {
            _service = resolver(ChartServiceType.ParkAreaPerPostcode);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _service.CreateAsync();

            return View(model);
        }

    }
}
