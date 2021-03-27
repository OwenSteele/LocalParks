﻿using AutoMapper;
using LocalParks.Core;
using LocalParks.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly SignInManager<LocalParksUser> _signInManager;
        private readonly UserManager<LocalParksUser> _userManager;

        public AccountService(IMapper mapper,
            SignInManager<LocalParksUser> signInManager,
            UserManager<LocalParksUser> userManager)
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<bool> SignInAttemptAsync(
            LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);

            return result.Succeeded;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<LocalParksUserModel> AddUserAsync(SignInModel model)
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

            //if (!await _parkRepository.SaveChangesAsync()) return null;

            var result = await _userManager.FindByNameAsync(user.UserName);

            return _mapper.Map<LocalParksUserModel>(result);
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
        public async Task<bool> ChangeDetailsAsync(ChangeDetailsModel model, string username)
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