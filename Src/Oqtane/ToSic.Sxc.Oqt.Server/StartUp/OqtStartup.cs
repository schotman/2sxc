﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oqtane.Extensions;
using Oqtane.Infrastructure;
using System.IO;
using ToSic.Eav.Configuration;
using ToSic.Eav.Run;
using ToSic.Eav.StartUp;
using ToSic.Eav.WebApi;
using ToSic.Lib.DI;
using ToSic.Razor.StartUp;
using ToSic.Sxc.DataSources;
using ToSic.Sxc.Oqt.Server.Adam.Imageflow;
using ToSic.Sxc.Oqt.Server.Controllers;
using ToSic.Sxc.Oqt.Server.Controllers.AppApi;
using ToSic.Sxc.Oqt.Shared;
using ToSic.Sxc.Razor;
using ToSic.Sxc.Startup;
using ToSic.Sxc.Web.EditUi;
using ToSic.Sxc.WebApi;
using WebApiConstants = ToSic.Sxc.Oqt.Server.WebApi.WebApiConstants;

namespace ToSic.Sxc.Oqt.Server.StartUp
{
    public class OqtStartup : IServerStartup
    {
        public IConfigurationRoot Configuration { get; }
        //public IWebHostEnvironment HostEnvironment { get; set; }

        public OqtStartup()
        {
            // Configuration is used to provide Master tenant sql connection string to 2sxc eav.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // 1. Enable dynamic razor compiling
            services.AddRazorPages()
                .AddRazorRuntimeCompilation(options =>
                {
                    var dllLocation = typeof(Oqtane.Server.Program).Assembly.Location;
                    var dllPath = Path.GetDirectoryName(dllLocation);
                    foreach (var dllFile in Directory.GetFiles(dllPath, "*.dll"))
                        options.AdditionalReferencePaths.Add(dllFile);
                });

            // TODO: STV - MAKE SURE OUR CONTROLLERS RULES ONLY APPLY TO OURS, NOT TO override rules on normal Oqtane controllers

            // 2. Register EAV & 2sxc
            services
                .AddSxcOqtane()                 // Always first add your override services
                .AddOqtSxcDataSources()
                .AddSxcRazor()                  // this is the .net core Razor compiler
                .AddAdamWebApi<int, int>()      // This is used to enable ADAM WebAPIs
                .AddSxcWebApi()                 // This adds all the standard backend services for WebAPIs to work
                .AddSxcCore()                   // Core 2sxc services
                .AddEav()                       // Core EAV services
                .AddEavWebApiTypedAfterEav<IActionResult>()
                .AddOqtAppWebApi()              // Oqtane App WebAPI stuff
                .AddRazorBlade();               // RazorBlade helpers for Razor in the edition used by Oqtane

            // 2sxc Oqtane blob services for Imageflow and other customizations.
            services.AddImageflowExtensions();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var serviceProvider = app.ApplicationServices;

            serviceProvider.Build<IDbConfiguration>().ConnectionString = Configuration.GetConnectionString("DefaultConnection");

            var globalConfig = serviceProvider.Build<IGlobalConfiguration>();
            globalConfig.GlobalFolder = Path.Combine(env.ContentRootPath, "wwwroot\\Modules", OqtConstants.PackageName);
            globalConfig.AppDataTemplateFolder = Path.Combine(env.ContentRootPath, "Content", "2sxc", "system", Eav.Constants.AppDataProtectedFolder, Eav.Constants.NewAppFolder);
            globalConfig.DataFolder = Path.Combine(env.ContentRootPath, "Content", "2sxc", "system", Eav.Constants.AppDataProtectedFolder, Eav.Constants.FolderSystem);
            globalConfig.TemporaryFolder = Path.Combine(env.ContentRootPath, "Content", "2sxc", "system", Eav.Constants.TemporaryFolder);
            globalConfig.InstructionsFolder = Path.Combine(env.ContentRootPath, "Content", "2sxc", "system", Eav.Constants.InstructionsFolder);
            globalConfig.AssetsVirtualUrl = $"~/Modules/{OqtConstants.PackageName}/assets/";
            globalConfig.SharedAppsFolder = $"/{OqtConstants.AppRoot}/{OqtConstants.SharedAppFolder}/"; // "/2sxc/Shared"

            // Load features from configuration
            // NOTE: On first installation of 2sxc module in oqtane, this code can not load all 2sxc global types
            // because it has dependency on ToSic_Eav_* sql tables, before this tables are actually created by oqtane 2.3.x,
            // but after next restart of oqtane application all is ok, and all 2sxc global types are loaded as expected

            var sxcSysLoader = serviceProvider.Build<SystemLoader>();
            sxcSysLoader.StartUp();

            if (env.IsDevelopment())
                app.UsePageResponseRewriteMiddleware();

            // MapWhen split the middleware pipeline into two completely separate branches
            app.MapWhen(context => 
                context.Request.Path.Value.Contains("/api/sxc/")
                || context.Request.Path.Value.Contains("/app/")
                || context.Request.Path.Value.Contains("/2sxc/shared/")
                || context.Request.Path.Value.Contains($"/Modules/{OqtConstants.PackageName}/dist/quickDialog/")
                || context.Request.Path.Value.Contains($"/Modules/{OqtConstants.PackageName}/dist/ng-edit/")
                , appBuilder =>
            {
                appBuilder.UseOqtaneMiddlewareConfiguration();

                // endpoint mapping
                appBuilder.UseEndpoints(endpoints =>
                {
                    // Release routes
                    endpoints.Map(WebApiConstants.AppRootNoLanguage + "/{appFolder}/api/{controller}/{action}", AppApiMiddleware.InvokeAsync);
                    endpoints.Map(WebApiConstants.AppRootNoLanguage + "/{appFolder}/{edition}/api/{controller}/{action}", AppApiMiddleware.InvokeAsync);
                    endpoints.Map(WebApiConstants.AppRootPathOrLang + "/{appFolder}/api/{controller}/{action}", AppApiMiddleware.InvokeAsync);
                    endpoints.Map(WebApiConstants.AppRootPathOrLang + "/{appFolder}/{edition}/api/{controller}/{action}", AppApiMiddleware.InvokeAsync);
                    endpoints.Map(WebApiConstants.AppRootPathNdLang + "/{appFolder}/api/{controller}/{action}", AppApiMiddleware.InvokeAsync);
                    endpoints.Map(WebApiConstants.AppRootPathNdLang + "/{appFolder}/{edition}/api/{controller}/{action}", AppApiMiddleware.InvokeAsync);
                    // Beta routes
                    endpoints.Map(WebApiConstants.WebApiStateRoot + "/app/{appFolder}/api/{controller}/{action}", AppApiMiddleware.InvokeAsync);
                    endpoints.Map(WebApiConstants.WebApiStateRoot + "/app/{appFolder}/{edition}/api/{controller}/{action}", AppApiMiddleware.InvokeAsync);
                    // Handle / Process URLs to Dialogs route for 2sxc UI
                    endpoints.MapFallback($"/Modules/{OqtConstants.PackageName}/dist/quickDialog/",
                        context => EditUiMiddleware.PageOutputCached(context, env, $@"Modules\{OqtConstants.PackageName}\dist\quickDialog\index-raw.html", EditUiResourceSettings.QuickDialog));
                    // Handle / Process URLs to Dialogs route for 2sxc UI
                    endpoints.MapFallback($"/Modules/{OqtConstants.PackageName}/dist/ng-edit/",
                        context => EditUiMiddleware.PageOutputCached(context, env, $@"Modules\{OqtConstants.PackageName}\dist\ng-edit\index-raw.html", EditUiResourceSettings.EditUi));
                });

                // end of this middleware pipeline branch
            });
        }
        
        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // Do nothing
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseOqtaneMiddlewareConfiguration(this IApplicationBuilder app)
        {
            // Oqtane middlewares should be executed before configuration of 2sxc endpoint mappings
            // to avoid having duplicate middleware in pipeline (like we had before).
            // Order of middleware configuration is important, because that is order of middleware execution in pipeline.

            #region Oqtane copy from Startup.cs - L173

            // allow oqtane localization middleware
            app.UseOqtaneLocalization();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseTenantResolution(); // commented, because it breaks alias resolving in 2sxc and it will resolve siteid=1 for all sites
            app.UseJwtAuthorization();
            app.UseBlazorFrameworkFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //if (_useSwagger)
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/" + Constants.Version + "/swagger.json", Constants.PackageId + " " + Constants.Version); });
            //}

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });

            #endregion
            return app;
        }
    }
}
