using LocalParks.Models;
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
        Task SignOutAsync();
        Task<object> GetUserTokenAsync(LoginModel model);
    }
}
