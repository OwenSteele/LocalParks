using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.ViewComponents
{
    public class EventsPerMonthChartViewComponent : ViewComponent
    {
        private readonly IViewComponentsService _service;

        public EventsPerMonthChartViewComponent(IViewComponentsService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _service.CreateEvents_Per_Month_ChartAsync();

            return View(model);
        }
    }
}
