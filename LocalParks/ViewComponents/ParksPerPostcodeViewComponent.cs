using LocalParks.Services.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.ViewComponents
{
    public class ParksPerPostcodeChartViewComponent : ViewComponent
    {
        private readonly IChartService _service;

        public ParksPerPostcodeChartViewComponent(ChartServiceResolver resolver)
        {
            _service = resolver(ChartServiceType.ParksPerPostcode);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _service.CreateAsync();

            return View(model);
        }
    }
}
