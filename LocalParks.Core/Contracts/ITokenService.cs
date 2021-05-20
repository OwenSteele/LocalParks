using LocalParks.Core.Models;
using LocalParks.Core.Models.Accounts;

namespace LocalParks.Core.Contracts
{
    public interface ITokenService
    {
        TokenModel CreateUserToken(LocalParksUserModel user, bool adminLifetime = false);
    }
}