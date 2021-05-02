using LocalParks.Core.Models.Account;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts
{
    public interface IAccountService
    {
        Task<bool> ChangePasswordAsync(string username, string newPassword);
        Task<bool> CheckPasswordAsync(string username, string password);
        Task<bool> DeleteUserAsync(string username, bool signOutUser = true);
        Task SignOutAsync();
        Task<bool> SignInAttemptAsync(LoginModel model);
    }
}