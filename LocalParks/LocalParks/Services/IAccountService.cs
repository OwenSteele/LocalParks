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
        Task<object> GetUserTokenAsync(LoginModel model);
        Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync();
        Task<LocalParksUserModel> AddUserAsync(SignInModel model, bool signInAfter = true);
        Task<bool> DeleteUserAsync(string username);
    }
}
