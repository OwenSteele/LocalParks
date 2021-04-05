using LocalParks.Services.Reports;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
