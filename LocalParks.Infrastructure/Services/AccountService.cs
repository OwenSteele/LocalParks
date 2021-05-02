using LocalParks.Core.Contracts;
using LocalParks.Core.Domain.User;
using LocalParks.Core.Models.Account;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LocalParks.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<LocalParksUser> _signInManager;
        private readonly UserManager<LocalParksUser> _userManager;

        public AccountService(SignInManager<LocalParksUser> signInManager,
            UserManager<LocalParksUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }


        public async Task<bool> DeleteUserAsync(string username, bool signOutUser = true)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return false;

            if (signOutUser) await _signInManager.SignOutAsync();

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(string username, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || !await _userManager.HasPasswordAsync(user))
                return false;

            var removed = await _userManager.RemovePasswordAsync(user);
            if (!removed.Succeeded) return false;

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);

            var updated = await _userManager.UpdateAsync(user);

            return updated.Succeeded;
        }
        public async Task<bool> CheckPasswordAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || !await _userManager.HasPasswordAsync(user))
                return false;

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            return result.Succeeded;
        }

        public async Task<bool> SignInAttemptAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                false,
                false);

            return result.Succeeded;
        }
    }
}