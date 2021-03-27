using LocalParks.Models;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IUserService
    {
        Task<LocalParksUserModel> GetUserAsync(string name);
        Task<LocalParksUserModel> GetUserByEmailAsync(string email);
    }
}