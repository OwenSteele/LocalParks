using LocalParks.Services.Reports;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Configuration.Injection
{
    public static class ReportsServicesCollectionExtension
    {
        public static IServiceCollection AddReportsServices(this IServiceCollection services)
        {
            services.AddScoped<IReportsDataService, ReportsDataService>();
            services.AddScoped<IReportsService, ReportsService>();

            return services;
        }
    }
}
