﻿using LocalParks.Core.Contracts;
using LocalParks.Core.Contracts.Managers;
using LocalParks.Infrastructure.Managers;
using LocalParks.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Infrastructure.DependencyInjection
{
    public static class LocalParksEntityServicesCollectionExtension
    {
        public static IServiceCollection AddLocalParksEntityServices(this IServiceCollection services)
        {
            services.AddScoped<IParkEventsManager, ParkEventsManager>();
            services.AddScoped<IParksService, ParksService>();
            services.AddScoped<IParkEventsService, ParkEventsService>();
            services.AddScoped<ISupervisorsService, SupervisorsService>();
            services.AddScoped<ISportsClubsService, SportsClubsService>();
            services.AddScoped<IPostcodesService, PostcodesService>();

            return services;
        }
    }
}
