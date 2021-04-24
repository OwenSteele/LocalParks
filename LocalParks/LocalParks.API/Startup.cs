using LocalParks.Data;
using LocalParks.Infrastructure;
using LocalParks.Infrastructure.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LocalParks.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddLocalParksData(Configuration.GetConnectionString("LocalParks"))
                 .AddLocalParksInfrastructure(Configuration.GetSection("Tokens"));

            services.AddAutoMapper(typeof(ParkProfile));

            services.AddCors();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(
                    options => options.WithOrigins("https://localparks.azurewebsites.net").AllowAnyMethod()
                    );    

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(
                    options => options.WithOrigins("https://localhost:44319").AllowAnyMethod()
                    );
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
