using LocalParks.Services;
using LocalParks.Services.Shop;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Configuration.Injection
{
    public static class ShopServicesCollectionExtension
    {
        public static IServiceCollection AddShopServices(this IServiceCollection services)
        {
            services.AddScoped<IShopManager, ShopManager>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderCreationService, OrderCreationService>();

            return services;
        }
    }
}
