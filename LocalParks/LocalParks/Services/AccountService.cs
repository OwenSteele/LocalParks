using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using LocalParks.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var user = await _userManager.FindByNameAsync(name);

            if (user == null) return null;

            var result = await _parkRepository.GetLocalParksUserByUsernameAsync(user.UserName);

            return _mapper.Map<LocalParksUserModel>(result);
        }
        public async Task<LocalParksUserModel> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return null;

            var result = await _parkRepository.GetLocalParksUserByEmailAsync(user.Email);

            return _mapper.Map<LocalParksUserModel>(result);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<TokenModel> GetUserTokenAsync(LocalParksUserModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null) return null;

            await _signInManager.SignInAsync(user, false);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenLifetime = 60 * (await _userManager.IsInRoleAsync(user, "Administrator") ? 3 : 1);

            var token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(tokenLifetime),
                signingCredentials: credentials
                );

            var results = new TokenModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiry = token.ValidTo.ToString()
            };

            return results;
        }
        public async Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync()
        {
            var postcodes = _mapper.Map<IEnumerable<PostcodeModel>>(await _parkRepository.GetAllPostcodesAsync());

            return from p in postcodes
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = p.Zone,
                       Value = p.Zone
                   };
        }

        public async Task<LocalParksUserModel> AddUserAsync(SignInModel model)
        {
            var existing = await _userManager.FindByNameAsync(model.Username);
            if(existing != null) return _mapper.Map<LocalParksUserModel>(existing);

            var user = new LocalParksUser
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PostcodeZone = model.PostcodeZone,
                MemberSince = DateTime.Today,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var created = await _userManager.CreateAsync(user, model.Password);
            if (!created.Succeeded)
            {
                return null;
            }

            //if (!await _parkRepository.SaveChangesAsync()) return null;

            var result = await _userManager.FindByNameAsync(user.UserName);

            return _mapper.Map<LocalParksUserModel>(result);
        }

        public async Task<bool> DeleteUserAsync(string username, bool signOutUser = true)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return false;

            if(signOutUser) await _signInManager.SignOutAsync();

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(string username, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(username);

            if(user == null || !await _userManager.HasPasswordAsync(user))
                return false;

            var removed = await _userManager.RemovePasswordAsync(user);
            if(!removed.Succeeded)return false;

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
        public async Task<bool> ChangeDetailsAsync(ChangeDetailsModel model, string username)
        {
            if (string.IsNullOrWhiteSpace(model.FirstName) &&
                string.IsNullOrWhiteSpace(model.LastName) &&
                string.IsNullOrWhiteSpace(model.PhoneNumber) &&
                string.IsNullOrWhiteSpace(model.PostcodeZone))
                return false;

            if (await CheckPostcodeExistsAsync(model.PostcodeZone)) 
                return false;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return false;

            if (!string.IsNullOrWhiteSpace(model.FirstName)) user.FirstName = model.FirstName;
            if (!string.IsNullOrWhiteSpace(model.LastName)) user.LastName = model.LastName;
            if (!string.IsNullOrWhiteSpace(model.PhoneNumber)) user.PhoneNumber = model.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(model.FirstName)) user.PostcodeZone = model.PostcodeZone;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
        public async Task<bool> CheckPostcodeExistsAsync(string postcodeZone)
        {
            return await _parkRepository.GetPostcodeByZoneAsync(postcodeZone) == null;
        }
        public async Task<ChangeDetailsModel> GetChangeDetailsModelAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null) return null;

            return new ChangeDetailsModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PostcodeZone = user.PostcodeZone

            };
        }
    }
}