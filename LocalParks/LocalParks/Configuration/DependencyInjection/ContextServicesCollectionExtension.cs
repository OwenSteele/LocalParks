using LocalParks.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
