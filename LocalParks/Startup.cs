using LocalParks.Configuration.DependencyInjection;
using LocalParks.Configuration.Injection;
using LocalParks.Configuration.Middleware;
using LocalParks.Contracts;
using LocalParks.Data;
using LocalParks.Infrastructure;
using LocalParks.Infrastructure.Models;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<QueryStringMiddlewareOptions>(Configuration.GetSection("QueryStrings"));

            services.AddLocalParksData(Configuration.GetConnectionString("LocalParks"))
                .AddLocalParksInfrastructure(Configuration.GetSection("Tokens"));

            services.AddDomainServices()
                .AddViewServices()
                .AddViewComponentServices()
                .AddReportsServices();

            services.AddMvc();

            services.AddAutoMapper(typeof(ParkProfile));

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseQueryStringMiddleware(
                new()
                {
                    EncryptionKey = Configuration["QueryStrings:Key"],
                    ParameterValue = Configuration["QueryStrings:Param"]
                });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
