using LocalParks.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
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
            services.AddDbContext<ParkContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("LocalParks"));
            });

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute(
                //     name: "lpparks",
                //     pattern: "API/{controller=LPParks}");
                //endpoints.MapControllerRoute(
                //     name: "lpsupervisor",
                //     pattern: "API/{controller=LPSupervisors}/");
                //endpoints.MapControllerRoute(
                //     name: "lpsportsclubs",
                //     pattern: "API/{controller=LPSportsClubs}/");
                //endpoints.MapControllerRoute(
                //     name: "lpparkevents",
                //     pattern: "API/{controller=LPParkEvents}/");
            });
        }
    }
}
