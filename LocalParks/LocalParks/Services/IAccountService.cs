using LocalParks.Models;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IAccountService
    {
        Task<LocalParksUserModel> AddUserAsync(SignInModel model);
        Task<bool> ChangeDetailsAsync(ChangeDetailsModel model, string username);
        Task<bool> ChangePasswordAsync(string username, string newPassword);
        Task<bool> CheckPasswordAsync(string username, string password);
        Task<bool> DeleteUserAsync(string username, bool signOutUser = true);
        Task<ChangeDetailsModel> GetChangeDetailsModelAsync(string name);
        Task<LocalParksUserModel> GetUserAsync(string name);
        Task<LocalParksUserModel> GetUserByEmailAsync(string email);
        Task<LocalParksUserModel> SignInAttemptAsync(LoginModel model);
        Task SignOutAsync();
    }
}