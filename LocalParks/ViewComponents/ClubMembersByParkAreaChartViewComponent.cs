using LocalParks.Services.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.ViewComponents
{
    public class ClubMembersByParkAreaChartViewComponent : ViewComponent
    {
        private readonly IChartService _service;

        public ClubMembersByParkAreaChartViewComponent(ChartServiceResolver resolver)
        {
            _service = resolver(ChartServiceType.MembersPerPark);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _service.CreateAsync();

            return View(model);
        }
    }
}
