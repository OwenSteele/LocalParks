using LocalParks.Models;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IHomeService
    {
        Task<HomeViewModel> GetHomeModelAsync(string latitude, string longitude);
        bool PostFeedBackAsync(ContactViewModel model);
    }
}