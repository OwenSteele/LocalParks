using System.Security.Claims;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IAuthenticationService
    {
        Task<bool> HasRequiredRoleAsync(string username, string role);
        Task<bool> IsSignedInAsync(ClaimsPrincipal user);
    }
}
