using LocalParks.Core.Models.Account;
using LocalParks.Core.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts.Managers
{
    public interface IAuthenticationManager
    {
        Task<TokenModel> AuthenticateAsync(LoginModel model);
        Task<TokenModel> AuthenticateContextAsync();
    }
}
