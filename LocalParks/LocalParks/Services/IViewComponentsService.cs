using LocalParks.Models.Chart;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IViewComponentsService
    {
        Task<ChartModel> CreateSupervisor_Salary_Tenure_ChartAsync();
        Task<ChartModel> CreateParks_Per_Postcode_ChartAsync();
        Task<ChartModel> CreateSports_Percentage_ChartAsync();
    }
}