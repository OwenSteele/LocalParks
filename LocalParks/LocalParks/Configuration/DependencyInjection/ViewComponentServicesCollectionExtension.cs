using LocalParks.Services;
using LocalParks.Services.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Configuration.Injection
{
    public static class ViewComponentServicesCollectionExtension
    {
        public static IServiceCollection AddViewComponentServices(this IServiceCollection services)
        {
            services.AddScoped<EventsPerMonthChartService>();
            services.AddScoped<ClubMembersPerParkSizeChartService>();
            services.AddScoped<ParksPerPostcodeChartService>();
            services.AddScoped<ParkAreaPostcodeChartService>();
            services.AddScoped<PopularShopProductsChartService>();
            services.AddScoped<SupervisorSalaryTenureChartService>();
            services.AddScoped<SportsPercentageChartService>();

            services.AddTransient<ChartServiceResolver>(serviceProvider => serviceTypeName =>
            {
                return serviceTypeName switch
                {
                    ChartServiceType.EventsPerMonth => serviceProvider.GetService<EventsPerMonthChartService>(),
                    ChartServiceType.MembersPerPark => serviceProvider.GetService<ClubMembersPerParkSizeChartService>(),
                    ChartServiceType.ParkAreaPerPostcode => serviceProvider.GetService<ParksPerPostcodeChartService>(),
                    ChartServiceType.ParksPerPostcode => serviceProvider.GetService<ParkAreaPostcodeChartService>(),
                    ChartServiceType.PopularShopProducts => serviceProvider.GetService<PopularShopProductsChartService>(),
                    ChartServiceType.SalaryTenure => serviceProvider.GetService<SupervisorSalaryTenureChartService>(),
                    ChartServiceType.SportsPercentage => serviceProvider.GetService<SportsPercentageChartService>(),
                    _ => null,
                };
            });

            return services;
        }
    }
}
