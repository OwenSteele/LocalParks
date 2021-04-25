using AutoMapper;
using LocalParks.Core.Contracts;
using LocalParks.Core.Domain.User;
using LocalParks.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace LocalParks.Services.Account
{
    public class GuestAccountService : IGuestAccountService
    {
        private readonly IMapper _mapper;
        private readonly SignInManager<LocalParksUser> _signInManager;
        private readonly UserManager<LocalParksUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IParkEventsService _parkEventsService;

        public GuestAccountService(IMapper mapper,
            SignInManager<LocalParksUser> signInManager,
            UserManager<LocalParksUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IParkEventsService parkEventsService)
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _parkEventsService = parkEventsService;
        }
        public async Task<bool> SignInAsync()
        {
            var dt = DateTime.Now.ToString("yy MM dd hh mm ss ffff");
            var username = $"LocalParksGuest{dt.Replace(" ", "")}";
            var user = new LocalParksUser
            {
                UserName = username,
                FirstName = "Guest",
                LastName = "User",
                PhoneNumber = "00000000000",
                MemberSince = DateTime.Now,
                Email = $"{username}@owensteele.co.uk",
                PostcodeZone = "LP1",
                PasswordHash = string.Empty
            };


            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded) return false;

            if (!await _roleManager.RoleExistsAsync("guest"))
                await _roleManager.CreateAsync(new IdentityRole { Id = "guest", Name = "guest" });

            var roleResult = await _userManager.AddToRoleAsync(user, "guest");

            if (!roleResult.Succeeded) return false;

            await _signInManager.SignInAsync(user, false);

            return true;
        }
        public async Task<bool> RemoveGuestAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);

            if (!await _userManager.IsInRoleAsync(user, "guest")) return false;

            if (user.OrganisedEvents != null)
            {
                foreach (var pe in user.OrganisedEvents)
                {
                    var model = _mapper.Map<ParkEventModel>(pe);
                    await _parkEventsService.DeleteParkEventAsync(model);
                }
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}
