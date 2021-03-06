using DevelopersChallengeNIBO.Interfaces.Database;
using DevelopersChallengeNIBO.Interfaces.Services;
using DevelopersChallengeNIBO.Models.Database;
using DevelopersChallengeNIBO.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Vereyon.Web;

namespace DevelopersChallengeNIBO
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
            // requires using Microsoft.Extensions.Options
            services.Configure<OFXRecordsDatabaseSettings>(
                Configuration.GetSection(nameof(OFXRecordsDatabaseSettings)));

            services.AddSingleton<IOFXRecordsDBSettings>(sp =>
                sp.GetRequiredService<IOptions<OFXRecordsDatabaseSettings>>().Value);

            services.AddSingleton<IOFXRecordsService, OFXRecordsService>();

            services.AddFlashMessage();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
