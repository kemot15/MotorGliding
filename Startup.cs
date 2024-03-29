using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MotorGliding.Context;
using MotorGliding.Models.Db;
using MotorGliding.Services;
using MotorGliding.Services.Interfaces;
using Rotativa.AspNetCore;

namespace MotorGliding
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var cs = Configuration.GetConnectionString("SQL");
            services.AddDbContext<MotorGlidingContext>(builder => builder.UseSqlServer(Configuration.GetConnectionString("SQL")));
            services.AddIdentity<User, IdentityRole<int>>().AddEntityFrameworkStores<MotorGlidingContext>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IFeaturesService, FeaturesService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)// IWebHostEnvironment IHostingEnvironment
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.SeedAdminUser();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
           // RotativaConfiguration.Setup(env);
           RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
