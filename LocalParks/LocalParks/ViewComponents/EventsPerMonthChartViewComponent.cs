using LocalParks.Services;
using LocalParks.Services.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.ViewComponents
{
    public class EventsPerMonthChartViewComponent : ViewComponent
    {
        private readonly IChartService _service;

        public EventsPerMonthChartViewComponent(ChartServiceResolver resolver)
        {
            _service = resolver(ChartServiceType.EventsPerMonth);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _service.CreateAsync();

            return View(model);
        }
    }
}
