using LocalParks.Models;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IHomeService
    {
        Task<HomeModel> GetHomeModelAsync(string latitude, string longitude);
    }
}