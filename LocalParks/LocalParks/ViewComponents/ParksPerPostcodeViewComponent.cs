using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.ViewComponents
{
    public class ParksPerPostcodeChartViewComponent : ViewComponent
    {
        private readonly IViewComponentsService _service;

        public ParksPerPostcodeChartViewComponent(IViewComponentsService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _service.CreateParks_Per_Postcode_ChartAsync();

            return View(model);
        }
    }
}
