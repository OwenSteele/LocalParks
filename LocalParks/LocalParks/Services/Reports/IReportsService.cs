using LocalParks.Models.Reports;
using System.Threading.Tasks;

namespace LocalParks.Services.Reports
{
    public interface IReportsService
    {
        Task<ReportsModel> GetReportsModelAsync();
    }
}