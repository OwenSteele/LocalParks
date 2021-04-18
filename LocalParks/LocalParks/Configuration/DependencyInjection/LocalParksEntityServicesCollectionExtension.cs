using LocalParks.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Configuration.Injection
{
    public static class LocalParksEntityServicesCollectionExtension
    {
        public static IServiceCollection AddLocalParksEntityServices(this IServiceCollection services)
        {
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IParksService, ParksService>();
            services.AddScoped<IParkEventsService, ParkEventsService>();
            services.AddScoped<ISupervisorsService, SupervisorsService>();
            services.AddScoped<ISportsClubsService, SportsClubsService>();
            services.AddScoped<IPostcodesService, PostcodesService>();

            return services;
        }
    }
}
