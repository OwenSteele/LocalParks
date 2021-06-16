using LocalParks.Core.Models;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts
{
    public interface IUserService
    {
        Task<LocalParksUserModel> GetUserAsync(string name);
        Task<LocalParksUserModel> GetUserByEmailAsync(string email);

    }
}