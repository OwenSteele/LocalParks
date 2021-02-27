using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class AccountService : IAccountService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        private readonly SignInManager<LocalParksUser> _signInManager;
        private readonly UserManager<LocalParksUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountService(IParkRepository parkRepository, IMapper mapper,
            SignInManager<LocalParksUser> signInManager,
            UserManager<LocalParksUser> userManager,
            IConfiguration configuration)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<LocalParksUserModel> SignInAttemptAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);

            if (!result.Succeeded) return null;

            var user = await _parkRepository.GetLocalParksUserByUsernameAsync(model.Username);
            return _mapper.Map<LocalParksUserModel>(user);
        }

        public async Task<LocalParksUserModel> GetUserAsync(string name)
        {
            var user = await _parkRepository.GetLocalParksUserByUsernameAsync(name);
            return _mapper.Map<LocalParksUserModel>(user);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<object> GetUserTokenAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null) return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) return null;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenLifetime = 20 * (await _userManager.IsInRoleAsync(user, "Administrator") ? 3 : 1);

            var token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(tokenLifetime),
                signingCredentials: credentials
                ) ;

            var results = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            };

            return results;
        }
    }
}