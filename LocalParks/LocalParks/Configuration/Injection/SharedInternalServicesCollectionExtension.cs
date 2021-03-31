using LocalParks.Services.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
