using LocalParks.Contracts;
using LocalParks.Services.View;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Configuration.Injection
{
    public static class ViewServicesCollectionExtension
    {
        public static IServiceCollection AddViewServices(this IServiceCollection services)
        {
            services.AddScoped<ISelectListService, SelectListService>();
            services.AddScoped<ISortingService, SortingService>();
            services.AddTransient<IUserInfoService, UserInfoService>();
            services.AddTransient<IParkEventInfoService, ParkEventInfoService>();
            services.AddTransient<ISupervisorInfoService, SupervisorInfoService>();

            return services;
        }
    }
}
