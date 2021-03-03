using LocalParks.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IAccountService
    {
        Task<LocalParksUserModel> SignInAttemptAsync(LoginModel model);
        Task<LocalParksUserModel> GetUserAsync(string name);
        Task<LocalParksUserModel> GetUserByEmailAsync(string email);
        Task SignOutAsync();
        Task<string[]> GetUserTokenAsync(LocalParksUserModel model);
        Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync();
        Task<LocalParksUserModel> AddUserAsync(SignInModel model);
        Task<bool> DeleteUserAsync(string username, bool signOutUser = true);
        Task<bool> ChangePasswordAsync(string username, string newPassword);
        Task<bool> CheckPasswordAsync(string username, string password);
        Task<bool> CheckPostcodeExistsAsync(string postcodeZone);
        Task<bool> ChangeDetailsAsync(ChangeDetailsModel model, string username);
        Task<ChangeDetailsModel> GetChangeDetailsModelAsync(string name);
    }
}
