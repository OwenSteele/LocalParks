using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class AccountService : IAccountService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        private readonly SignInManager<LocalParksUser> _signInManager;

        public AccountService(IParkRepository parkRepository, IMapper mapper,
            SignInManager<LocalParksUser> signInManager)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
            _signInManager = signInManager;
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
    }
}
