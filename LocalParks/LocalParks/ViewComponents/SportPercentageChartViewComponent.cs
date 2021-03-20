using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.ViewComponents
{
    public class SportPercentageChartViewComponent : ViewComponent
    {
        private readonly IViewComponentsService _service;

        public SportPercentageChartViewComponent(IViewComponentsService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _service.CreateSports_Percentage_ChartAsync();

            return View(model);
        }
    }
}
