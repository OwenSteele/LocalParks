using LocalParks.Core.Models;
using LocalParks.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services.Account
{
    public interface IAccountDataService
    {
        Task<LocalParksUserModel> AddUserAsync(SignInViewModel model);
        Task<bool> ChangeDetailsAsync(ChangeDetailsViewModel model, string username);
        Task<ChangeDetailsViewModel> GetChangeDetailsModelAsync(string name);
        Task<bool> SignInAttemptAsync(LoginViewModel model);

    }
}
