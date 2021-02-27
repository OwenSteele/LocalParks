using LocalParks.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<LocalParksUser> _userManager;
        private readonly RoleManager<LocalParksUser> _roleManager;

        public AuthenticationService(UserManager<LocalParksUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> HasRequiredRoleAsync(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}
