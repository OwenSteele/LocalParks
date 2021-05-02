using AutoMapper;
using LocalParks.Core.Domain.User;
using LocalParks.Core.Models;
using LocalParks.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace LocalParks.Services.Account
{
    public class AccountDataService : IAccountDataService
    {
        private readonly IMapper _mapper;
        private readonly SignInManager<LocalParksUser> _signInManager;
        private readonly UserManager<LocalParksUser> _userManager;

        public AccountDataService(IMapper mapper,
            SignInManager<LocalParksUser> signInManager,
            UserManager<LocalParksUser> userManager)
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<bool> SignInAttemptAsync(
            LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);

            return result.Succeeded;
        }
        public async Task<LocalParksUserModel> AddUserAsync(SignInViewModel model)
        {
            var existing = await _userManager.FindByNameAsync(model.Username);
            if (existing != null) return _mapper.Map<LocalParksUserModel>(existing);

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

            var result = await _userManager.FindByNameAsync(user.UserName);

            return _mapper.Map<LocalParksUserModel>(result);
        }
        public async Task<bool> ChangeDetailsAsync(ChangeDetailsViewModel model, string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return false;

            if (!string.IsNullOrWhiteSpace(model.FirstName)) user.FirstName = model.FirstName;
            if (!string.IsNullOrWhiteSpace(model.LastName)) user.LastName = model.LastName;
            if (!string.IsNullOrWhiteSpace(model.PhoneNumber)) user.PhoneNumber = model.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(model.FirstName)) user.PostcodeZone = model.PostcodeZone;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
        public async Task<ChangeDetailsViewModel> GetChangeDetailsModelAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null) return null;

            return new ChangeDetailsViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PostcodeZone = user.PostcodeZone

            };
        }
    }
}
