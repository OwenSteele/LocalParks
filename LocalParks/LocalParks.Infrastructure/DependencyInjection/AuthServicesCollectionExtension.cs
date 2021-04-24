using LocalParks.Core.Contracts;
using LocalParks.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Infrastructure.DependencyInjection
{
    public static class AuthServicesCollectionExtension
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
