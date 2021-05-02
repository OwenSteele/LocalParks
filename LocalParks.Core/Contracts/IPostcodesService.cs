using LocalParks.Core.Models;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts
{
    public interface IPostcodesService
    {
        Task<PostcodeModel[]> GetAllPostcodesAsync();
        Task<PostcodeModel> GetPostcodeAsync(string zone);
    }
}