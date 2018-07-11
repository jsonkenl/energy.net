using Energy.Core;
using Energy.Core.Interfaces;
using Energy.Infrastructure;
using Energy.Infrastructure.SeedData;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            // Configure Authentication Service
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = Configuration.GetValue<string>("Cookies:SessionPath");
                    options.Cookie.Name = Configuration.GetValue<string>("Cookies:Name");
                    options.AccessDeniedPath = Configuration.GetValue<string>("Cookies:ForbiddenPath");
                });

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

            // Bind ApplicationOptions Class to Configuration
            services.Configure<ApplicationOptions>(options => Configuration.GetSection("ApplicationOptions").Bind(options));

            // DbContext Setup and Implementation
            if (_currentEnvironment.IsDevelopment())
            {
                services.AddDbContext<EnergyDotNetDbContext>(
                    options => options.UseInMemoryDatabase("EnergyDotNet_Dev"));

                InjectDatabaseDependencies(services);
            }
            else
            {
                services.AddDbContext<EnergyDotNetDbContext>(
                    options => options.UseSqlServer(Configuration.GetConnectionString("EnergyDotNet")));

                InjectDatabaseDependencies(services);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                                AdminEmployeeSeedData adminSeedData)
        {
            // Cookie Based Authentication
            app.UseAuthentication();

            // Exception Handling
            if (_currentEnvironment.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // File Serving, Temp Storage and Routes
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Database Seeding
            var adminEmail = Configuration.GetValue<string>("ApplicationOptions:AdministratorEmail");
            var adminDn = Configuration.GetValue<string>("ApplicationOptions:AdministratorDistinguishedName");

            adminSeedData.EnsureAdminEmployee(adminEmail, adminDn).Wait();
        }

        private void InjectDatabaseDependencies(IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IAdministratorRepository, AdministratorRepository>();

            services.AddTransient<AdminEmployeeSeedData>();
        }
    }
}
