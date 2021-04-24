using LocalParks.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LocalParks.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {        public static IServiceCollection AddLocalParksInfrastructure(this IServiceCollection services, IConfigurationSection configuration)
        {
            services
                .AddAuthServices()
                .AddLocalParksEntityServices()
                .AddShopServices()
                .AddInternalSharedServices()
                .AddSecurityServices();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = configuration.GetValue<string>("Issuer"),
                        ValidAudience = configuration.GetValue<string>("Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration.GetValue<string>("Key")))
                    };
                });

            return services;
        }
    }
}
