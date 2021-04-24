using LocalParks.Core.Contracts;
using LocalParks.Infrastructure.Services.View;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Infrastructure.DependencyInjection
{
    public static class SecurityServicesCollectionExtension
    {
        public static IServiceCollection AddSecurityServices(this IServiceCollection services)
        {
            services.AddSingleton<IEncryptionService, EncryptionService>();

            return services;
        }
    }
}
