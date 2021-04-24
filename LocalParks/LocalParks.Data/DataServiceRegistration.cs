using LocalParks.Core.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LocalParks.Data
{
    public static class DataServiceRegistration
    {
        public static IServiceCollection AddLocalParksData(this IServiceCollection services, string connectionString)
        {
            services.AddIdentity<LocalParksUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }
               ).AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ParkContext>();

            services.AddDbContext<ParkContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddTransient<IParksSeeder, ParksSeeder>();

            services.AddScoped<IParkRepository, ParkRepository>();

            
            return services;
        }
    }
}
