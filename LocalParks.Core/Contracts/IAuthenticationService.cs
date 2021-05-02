using System.Security.Claims;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts
{
    public interface IAuthenticationService
    {
        Task<bool> HasRequiredRoleAsync(string username, string role);
        Task<bool> IsSignedInAsync(ClaimsPrincipal user);
    }
}
