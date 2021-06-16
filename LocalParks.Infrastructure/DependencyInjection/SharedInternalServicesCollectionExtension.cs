using LocalParks.Core.Contracts.Shared;
using LocalParks.Infrastructure.Services.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Infrastructure.DependencyInjection
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
