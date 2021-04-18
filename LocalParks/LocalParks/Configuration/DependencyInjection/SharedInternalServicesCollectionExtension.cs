using LocalParks.Services.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Configuration.Injection
{
    public static class SharedInternalServicesCollectionExtension
    {

        public static IServiceCollection AddInternalSharedServices(this IServiceCollection services)
        {
            services.AddSingleton<IRandomService, RandomService>();

            return services;
        }
    }
}
