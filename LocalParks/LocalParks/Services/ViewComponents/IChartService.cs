using LocalParks.Models.Chart;
using System.Threading.Tasks;

namespace LocalParks.Services.ViewComponents
{
    public interface IChartService
    {
        Task<ChartModel> CreateAsync();
    }
}