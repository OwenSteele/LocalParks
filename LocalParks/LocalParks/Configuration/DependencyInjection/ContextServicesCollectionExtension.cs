using LocalParks.Data;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Configuration.Injection
{
    public static class ContextServicesCollectionExtension
    {
        public static IServiceCollection AddLocalParksContextServices(this IServiceCollection services)
        {
            services.AddTransient<IParksSeeder, ParksSeeder>();

            services.AddScoped<IParkRepository, ParkRepository>();

            return services;
        }
    }
}
