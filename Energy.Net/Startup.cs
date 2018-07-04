using Energy.Core.Interfaces;
using Energy.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Energy.Net
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _currentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment _currentEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // MVC with Feature Folders
            services.AddMvc(o => o.Conventions.Add(new FeatureControllerConvention()))
                .AddRazorOptions(options =>
                {
                    // {0} - Action Name
                    // {1} - Controller Name
                    // {2} - Feature Name
                    // Replace normal view location entirely
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Features/{2}/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/{2}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                    options.ViewLocationExpanders.Add(new FeaturesViewLocationExpander());
                });

            // DbContext Setup and Implementation
            if (_currentEnvironment.IsDevelopment())
            {
                InitiateDatabaseAccess(services, "EnergyDotNet_Dev");
            }
            else
            {
                InitiateDatabaseAccess(services, "EnergyDotNet");
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_currentEnvironment.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitiateDatabaseAccess(IServiceCollection services, string databaseName)
        {
            services.AddDbContext<EnergyDotNetDbContext>(
                    options => options.UseSqlServer(Configuration.GetConnectionString(databaseName)));

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        }
    }
}
