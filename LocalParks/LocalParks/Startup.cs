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
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

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
            }
            ).AddRoles<IdentityRole>()
             .AddRoleManager<RoleManager<IdentityRole>>()
             .AddEntityFrameworkStores<ParkContext>();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };
                });

            services.AddDbContext<ParkContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LocalParks"));
            });

            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IParksService, ParksService>();
            services.AddTransient<IParkEventsService, ParkEventsService>();
            services.AddTransient<ISupervisorsService, SupervisorsService>();
            services.AddTransient<ISportsClubsService, SportsClubsService>();
            services.AddTransient<IShopService, ShopService>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddMvc();

            services.AddTransient<ParksSeeder>();

            services.AddScoped<IParkRepository, ParkRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

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


            app.UseRouting();

            app.UseAuthentication();

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
