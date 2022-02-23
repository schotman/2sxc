﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using ToSic.Eav.Context;
using ToSic.Eav.WebApi.ImportExport;
using ToSic.Sxc.Context;
using ToSic.Sxc.Dnn.Context;
using ToSic.Sxc.Run;

namespace ToSic.Sxc.Dnn.WebApi.Sys
{
    public class InstallController : DnnApiControllerWithFixes<IEnvironmentInstaller>
    {
        public InstallController() : base("Install") { }

        /// <summary>
        /// Make sure that these requests don't land in the normal api-log.
        /// Otherwise each log-access would re-number what item we're looking at
        /// </summary>
        protected override string HistoryLogGroup => "web-api.install";

        #region System Installation

        /// <summary>
        /// Finish system installation which had somehow been interrupted
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Host)]
        public bool Resume() => Real.ResumeAbortedUpgrade();

        #endregion


        #region App / Content Package Installation

        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Admin)]
        public string RemoteWizardUrl(bool isContentApp) =>
            Real.GetAutoInstallPackagesUiUrl(
                    GetService<ISite>(), ((DnnModule)GetService<IModule>()).Init(Request.FindModuleInfo(), Log), 
                    isContentApp);


        /// <summary>
        /// Before this was GET Installer/InstallPackage
        /// </summary>
        /// <param name="packageUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Admin)]
        [ValidateAntiForgeryToken] // now activate this, as it's post now, previously not, because this is a GET and can't include the RVT
        public HttpResponseMessage RemotePackage(string packageUrl)
        {
            PreventServerTimeout300();

            Log.Add("install package:" + packageUrl);
            var module = ((DnnModule)GetService<IModule>()).Init(ActiveModule, Log);
            var block = module.BlockIdentifier;

            var importFromRemote = GetService<ImportFromRemote>();

            var fromRemote = importFromRemote.Init(new DnnUser(), Log);

            var result = fromRemote
                .InstallPackage(block.ZoneId, block.AppId, ActiveModule.DesktopModule.ModuleName == "2sxc-app", packageUrl);

            Log.Add("install completed with success:" + result.Item1);
            return Request.CreateResponse(result.Item1 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError, new { result.Item1, result.Item2 });
        }

        #endregion
    }
}
