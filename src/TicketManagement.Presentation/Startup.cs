using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using RestEase;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.Settings;

namespace TicketManagement.Presentation
{
    /// <summary>
    /// Sets configuration for MVC application.
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Installation services for application. Added database-context, identity-context, password settings.
        /// </summary>
        /// <param name="services">collection of services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews().AddDataAnnotationsLocalization()
                .AddViewLocalization();
            services.AddFeatureManagement().
                UseDisabledFeaturesHandler(new RedirectDisabledFeatureHandler(Configuration));
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("be-BY"),
                    new CultureInfo("ru"),
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddScoped(scope =>
            {
                var baseUrl = Configuration["UserApiAddress"];
                return RestClient.For<IUserRestClient>(baseUrl);
            });
            services.AddScoped(scope =>
            {
                var baseUrl = Configuration["EventApiAddress"];
                return RestClient.For<IEventRestClient>(baseUrl);
            });
            services.AddScoped(scope =>
            {
                var baseUrl = Configuration["VenueApiAddress"];
                return RestClient.For<IVenueRestClient>(baseUrl);
            });
            services.AddScoped(scope =>
            {
                var baseUrl = Configuration["TicketApiAddress"];
                return RestClient.For<ITicketRestClient>(baseUrl);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });
        }

        /// <summary>
        /// Installation midlleware for application. Added defaults for MVC middleware, middleware for authentification and authorization.
        /// </summary>
        /// <param name="app">application builde.</param>
        /// <param name="env">web host environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler(
                 options =>
                 {
                     options.Run(
                     async context =>
                     {
                         context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                         context.Response.ContentType = "text/html";
                         var ex = context.Features.Get<IExceptionHandlerFeature>();
                         if (ex != null)
                         {
                             context.Response.Cookies.Append("message", "No access");
                             await Task.Run(() => context.Response.Redirect("/Home/ErrorMessage"));
                         }
                     });
                 });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRequestLocalization();

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
