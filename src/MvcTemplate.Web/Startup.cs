using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Routing;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Data.Migrations;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NonFactors.Mvc.Grid;
using System;
using System.IO;

namespace MvcTemplate.Web
{
    public class Startup
    {
        private String ApplicationBasePath { get; }

        public Startup(IApplicationEnvironment env)
        {
            ApplicationBasePath = env.ApplicationBasePath;
        }
        public void Configure(IApplicationBuilder app)
        {
            RegisterAppServices(app);
            RegisterRoute(app);

            SeedData(app);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterCurrentDependencyResolver(services);
            RegisterLowercaseUrls(services);
            RegisterFilters(services);
            RegisterMvcGrid(services);
            RegisterSession(services);
            RegisterMvc(services);
        }
        
        public virtual void RegisterCurrentDependencyResolver(IServiceCollection services)
        {
            services.AddTransient<DbContext, Context>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ILogger, Logger>();
            services.AddTransient<IAuditLogger, AuditLogger>();

            services.AddTransient<IHasher, BCrypter>();
            services.AddTransient<IMailClient>(provider => new SmtpMailClient("smtp.gmail.com", 587, "MVC.Template@gmail.com", "ChangeIt"));

            services.AddTransient<IExceptionFilter, ExceptionFilter>();
            services.AddTransient<IModelMetadataProvider, DisplayNameMetadataProvider>();

            services.AddSingleton<IGlobalizationProvider>(provider =>
                new GlobalizationProvider(Path.Combine(ApplicationBasePath, "Globalization.xml")));
            services.AddSingleton<IAuthorizationProvider>(provider => new AuthorizationProvider(typeof(BaseController).Assembly, provider));

            services.AddTransient<IMvcSiteMapParser, MvcSiteMapParser>();
            services.AddSingleton<IMvcSiteMapProvider>(provider => new MvcSiteMapProvider(
                Path.Combine(ApplicationBasePath, "Mvc.sitemap"), provider.GetService<IMvcSiteMapParser>(), provider.GetService<IAuthorizationProvider>()));

            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddTransient<IRoleValidator, RoleValidator>();
            services.AddTransient<IAccountValidator, AccountValidator>();
        }
        public virtual void RegisterLowercaseUrls(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        }
        public virtual void RegisterFilters(IServiceCollection services)
        {
            services.Configure<MvcOptions>(options => options.Filters.Add(typeof(ExceptionFilter)));
        }
        public virtual void RegisterMvcGrid(IServiceCollection services)
        {
            services.AddMvcGrid();
        }
        public virtual void RegisterSession(IServiceCollection services)
        {
            services.AddCaching();
            services.AddSession();
        }
        public virtual void RegisterMvc(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddMvcOptions(options => options.ModelBinders.Insert(0, new TrimmingModelBinder()))
                .AddRazorOptions(options => options.ViewLocationExpanders.Add(new ViewLocationExpander()));
        }

        public virtual void RegisterAppServices(IApplicationBuilder app)
        {
            app.UseCookieAuthentication(options =>
            {
                options.LoginPath = "/auth/login";
                options.AutomaticChallenge = true;
                options.AutomaticAuthenticate = true;
                options.AuthenticationScheme = "Cookies";
            });
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseSession();
        }
        public virtual void RegisterRoute(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "DefaultMultilingualArea",
                    "{language}/{area:exists}/{controller}/{action}/{id?}",
                    new { controller = "Home", action = "Index" },
                    new { language = "lt" });

                routes.MapRoute(
                    "DefaultArea",
                    "{area:exists}/{controller}/{action}/{id?}",
                    new { language = "en", controller = "Home", action = "Index" },
                    new { language = "en" });

                routes.MapRoute(
                    "DefaultMultilingual",
                    "{language}/{controller}/{action}/{id?}",
                    new { controller = "Home", action = "Index" },
                    new { language = "lt" });

                routes.MapRoute(
                    "Default",
                    "{controller}/{action}/{id?}",
                    new { language = "en", controller = "Home", action = "Index" },
                    new { language = "en" });
            });
        }

        public virtual void SeedData(IApplicationBuilder app)
        {
            using (Configuration configuration = new Configuration(app.ApplicationServices.GetService<DbContext>()))
                configuration.Seed();
        }
    }
}
