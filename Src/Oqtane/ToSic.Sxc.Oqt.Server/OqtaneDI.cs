﻿using System;
using Microsoft.Extensions.DependencyInjection;
using ToSic.Eav.Apps;
using ToSic.Eav.Apps.Security;
using ToSic.Eav.LookUp;
using ToSic.Eav.Persistence.Interfaces;
using ToSic.Eav.Run;
using ToSic.Sxc.Adam;
using ToSic.Sxc.Code;
using ToSic.Sxc.Oqt.Server.Adam;
using ToSic.Sxc.Oqt.Server.Code;
using ToSic.Sxc.Oqt.Server.Controllers;
using ToSic.Sxc.Oqt.Server.Controllers.Adam;
using ToSic.Sxc.Oqt.Server.Page;
using ToSic.Sxc.Oqt.Server.Plumbing;
using ToSic.Sxc.Oqt.Server.Run;
using ToSic.Sxc.Oqt.Shared.Run;
using ToSic.Sxc.Run;
using ToSic.Sxc.Web;
using ToSic.Sxc.WebApi.Adam;

namespace ToSic.Sxc.Oqt.Server
{
    // ReSharper disable once InconsistentNaming
    static class OqtaneDI
    {
        public static IServiceCollection AddSxcOqtane(this IServiceCollection services)
        {
            services.AddScoped<ILinkPaths, OqtLinkPaths>();
            services.AddTransient<IServerPaths, OqtServerPaths>();


            services.AddTransient<IAppEnvironment, OqtEnvironment>();
            services.AddTransient<IEnvironment, OqtEnvironment>();
            services.AddTransient<ISite, OqtSite>();
            services.AddTransient<IRenderingHelper, OqtRenderingHelper>();
            services.AddTransient<IZoneMapper, OqtZoneMapper>();
            services.AddTransient<AppPermissionCheck, OqtPermissionCheck>();
            services.AddTransient<DynamicCodeRoot, OqtaneDynamicCode>();
            services.AddTransient<IPlatformModuleUpdater, OqtModuleUpdater>();
            services.AddTransient<IEnvironmentInstaller, OqtEnvironmentInstaller>();
            services.AddTransient<IGetEngine, OqtGetLookupEngine>();
            services.AddTransient<OqtContextBuilder>();
            services.AddTransient<OqtContainer>();
            services.AddTransient<OqtTempInstanceContext>();
            services.AddTransient<OqtSite>();
            services.AddTransient<OqtZoneMapper>();
            services.AddTransient<SettingsHelper>();
            //// add page publishing
            services.AddTransient<IPagePublishing, OqtPagePublishing>();

            //// Oqtane Specific stuff
            services.AddScoped<OqtAssetsAndHeaders>();
            services.AddTransient<OqtSiteFactory>();
            services.AddTransient<SxcOqtane>();
            services.AddTransient<IClientDependencyOptimizer, OqtClientDependencyOptimizer>();
            services.AddTransient<IValueConverter, OqtValueConverter>();

            services.AddSingleton<Sxc.Run.Context.PlatformContext, OqtPlatformContext>();

            services.AddTransient<SecurityChecksBase, OqtAdamSecurityChecks>();
            services.AddTransient<IAdamFileSystem<int, int>, OqtAdamFileSystem>();
            services.AddTransient(typeof(AdamItemDtoMaker<,>), typeof(OqtAdamItemDtoMaker<,>));

            //// Add SxcEngineTest
            //services.AddTransient<SxcMvc>();
            //// Still pending...
            ////sc.AddTransient<XmlExporter, DnnXmlExporter>();
            services.AddTransient<IImportExportEnvironment, OqtImportExportEnvironment>();
            //sc.AddTransient<IAppFileSystemLoader, DnnAppFileSystemLoader>();
            //sc.AddTransient<IAppRepositoryLoader, DnnAppFileSystemLoader>();

            // 2020-10-22 2dm test
            services.AddTransient<ISxcOqtane, SxcOqtane>();
            //services.AddTransient<IRenderRazor, RenderRazor>();
            //services.AddTransient<IEngineFinder, OqtaneEngineFinder>();
            services.AddTransient<StatefulControllerDependencies>();

            // Plumbing: enable lazy services Dependency Injection

            return services;
        }



    }
}
