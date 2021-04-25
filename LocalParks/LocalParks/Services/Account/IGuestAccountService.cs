using LocalParks.Core.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LocalParks.Services.Account
{
    public interface IGuestAccountService
    {
        Task<bool> SignInAsync();
        Task<bool> RemoveGuestAsync(string name);
    }
}