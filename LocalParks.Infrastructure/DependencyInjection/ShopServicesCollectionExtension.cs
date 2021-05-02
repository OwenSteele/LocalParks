using LocalParks.Core.Contracts;
using LocalParks.Infrastructure.Services.Shop;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Infrastructure.DependencyInjection
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
