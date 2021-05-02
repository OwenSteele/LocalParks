using LocalParks.Core.Contracts;
using LocalParks.Core.Domain.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LocalParks.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<LocalParksUser> _userManager;

        public AuthenticationService(UserManager<LocalParksUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> HasRequiredRoleAsync(string username, string role)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;

            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            return await _userManager.IsInRoleAsync(user, role);
        }
        public async Task<bool> IsSignedInAsync(ClaimsPrincipal user)
        {
            if (user == null || user.Identity.Name == null) return false;

            return await _userManager.FindByNameAsync(user.Identity.Name) != null;
        }
    }
}
