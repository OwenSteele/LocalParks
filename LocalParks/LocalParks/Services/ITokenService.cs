using LocalParks.Models;
using LocalParks.Models.Accounts;

namespace LocalParks.Services
{
    public interface ITokenService
    {
        TokenModel CreateUserToken(LocalParksUserModel user, bool adminLifetime = false);
    }
}