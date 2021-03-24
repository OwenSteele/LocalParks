using LocalParks.Models;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IPostcodesService
    {
        Task<PostcodeModel[]> GetAllPostcodesAsync();
        Task<PostcodeModel> GetPostcodeAsync(string zone);
    }
}
