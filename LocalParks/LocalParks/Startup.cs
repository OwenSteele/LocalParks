using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace LocalParks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<LocalParksUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                // add tokens
            }
            ).AddEntityFrameworkStores<ParkContext>();

            services.AddDbContext<ParkContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LocalParks"));
            });

            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IParksService, ParksService>();
            services.AddTransient<IParkEventsService, ParkEventsService>();
            services.AddTransient<ISupervisorsService, SupervisorsService>();
            services.AddTransient<ISportsClubsService, SportsClubsService>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddMvc();

            services.AddTransient<ParksPartialSeeder>();

            services.AddScoped<IParkRepository, ParkRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
