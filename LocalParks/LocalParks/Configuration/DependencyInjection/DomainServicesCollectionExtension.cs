using LocalParks.Services;
using LocalParks.Services.Account;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Configuration.DependencyInjection
{
    public static class DomainServicesCollectionExtension
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IAccountDataService, AccountDataService>();

            return services;
        }
    }
}
