using LocalParks.Core.Contracts;
using LocalParks.Core.Contracts.Managers;
using LocalParks.Core.Models.Account;
using LocalParks.Core.Models.Accounts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Infrastructure.Managers
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        public AuthenticationManager(IAccountService accountService,
            ITokenService tokenService, IUserService userService,
            IHttpContextAccessor contextAccessor)
        {
            _userService = userService;
            _contextAccessor = contextAccessor;
            _accountService = accountService;
            _tokenService = tokenService;
        }
        public async Task<TokenModel> AuthenticateAsync(LoginModel model)
        {
            if (!await _accountService.SignInAttemptAsync(model))
                throw new BadHttpRequestException("Invalid login details");

            var user = await _userService.GetUserAsync(model.Username);

            return _tokenService.CreateUserToken(user);
        }
        public async Task<TokenModel> AuthenticateContextAsync()
        {
            var username = _contextAccessor.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrWhiteSpace(username)) throw new UnauthorizedAccessException();

            return await GetTokenFromUsernameAsync(username);
        }

        private async Task<TokenModel> GetTokenFromUsernameAsync(string username)
        {
            var user = await _userService.GetUserAsync(username);

            return _tokenService.CreateUserToken(user);  
        }
    }
}
