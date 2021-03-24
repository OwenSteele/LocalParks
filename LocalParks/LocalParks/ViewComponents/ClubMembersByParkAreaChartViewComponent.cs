using LocalParks.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalParks.ViewComponents
{
    public class ClubMembersByParkAreaChartViewComponent : ViewComponent
    {
        private readonly IViewComponentsService _service;

        public ClubMembersByParkAreaChartViewComponent(IViewComponentsService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _service.CreateClubMembers_Per_ParkSize_ChartAsync();

            return View(model);
        }
    }
}
