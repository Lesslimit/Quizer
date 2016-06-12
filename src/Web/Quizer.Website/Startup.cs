using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Serialization;
using Quizer.Common.JsonConverters;
using Quizer.DataAccess.AzureStorage;
using Quizer.DataAccess.DocumentDb;
using Quizer.Security;
using Quizer.Security.Identity;
using SimpleInjector;
using SimpleInjector.Integration.AspNet;

namespace Quizer.Websiite
{
    public class Startup
    {
        private readonly Container container = new Container();

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(container));

            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(container));

            services.AddSingleton(sp => container.GetInstance<CloudBlobClient>());

            services.AddIdentity<QuizerUser, string>()
                .AddUserStore<UserStore>();

            services.AddDataProtection();

            services.AddTransient(sp => container.GetInstance<IStorage>());

            services.AddOptions();

            services.Configure<DocumentDbOptions>(Configuration.GetSection("DocumentDb"), true);
            services.Configure<AzureStorageOptions>(Configuration.GetSection("AzureStorage"), true);

            services.AddMvc(options =>
            {
                //var jsonInputFormatter = options.InputFormatters.OfType<JsonInputFormatter>().Single(jif => jif.SupportedMediaTypes.Any(mt => mt == "application/json"));

                //jsonInputFormatter.SerializerSettings.Converters.Add(new ClaimConverter());
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Converters.Add(new ClaimConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            container.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();
            app.UseSimpleInjectorAspNetRequestScoping(container);

            InitializeContainer(app);

            container.Verify();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "website",
                SlidingExpiration = true,
                AutomaticChallenge = true,
                CookieName = "Quizer.Website",
                TicketDataFormat = new SecureDataFormat<AuthenticationTicket>(TicketSerializer.Default, app.ApplicationServices.GetDataProtector("Cookie"))
            });

            app.UseIdentity();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Cross-wire ASP.NET services (if any). For instance:
            container.CrossWire<ILoggerFactory>(app);
            container.CrossWire<IDataProtectionProvider>(app);
            container.CrossWire<UserManager<QuizerUser>>(app);

            container.Register(app.ApplicationServices.GetService<IOptions<DocumentDbOptions>>, Lifestyle.Singleton);
            container.Register(app.ApplicationServices.GetService<IOptions<AzureStorageOptions>>, Lifestyle.Singleton);

            container.RegisterPackages(new[] { typeof(Startup).Assembly });
        }
    }
}
