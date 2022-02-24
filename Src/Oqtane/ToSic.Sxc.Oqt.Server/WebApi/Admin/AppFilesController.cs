﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Shared;
using ToSic.Eav.WebApi.Assets;
using ToSic.Eav.WebApi.Routing;
using ToSic.Sxc.Apps.Assets;
using ToSic.Sxc.Oqt.Server.Controllers;
using ToSic.Sxc.WebApi.Admin.AppFiles;

namespace ToSic.Sxc.Oqt.Server.WebApi.Admin
{
    /// <summary>
    /// This one supplies portal-wide (or cross-portal) settings / configuration
    /// </summary>
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleNames.Admin)]

    // Release routes
    [Route(WebApiConstants.ApiRootWithNoLang + $"/{AreaRoutes.Admin}")]
    [Route(WebApiConstants.ApiRootPathOrLang + $"/{AreaRoutes.Admin}")]
    [Route(WebApiConstants.ApiRootPathNdLang + $"/{AreaRoutes.Admin}")]

    // Beta routes - TODO: @STV - why is this beta?
    [Route(WebApiConstants.WebApiStateRoot + $"/{AreaRoutes.Admin}")]
    public class AppFilesController : OqtStatefulControllerBase<AppFilesControllerReal>, IAppFilesController
    {
        public AppFilesController(): base(AppFilesControllerReal.LogSuffix) { }


        [HttpGet]
        public List<string> All(
            [FromQuery] int appId,
            [FromQuery] bool global,
            [FromQuery] string path = null,
            [FromQuery] string mask = "*.*",
            [FromQuery] bool withSubfolders = false,
            [FromQuery] bool returnFolders = false
        ) => Real.All(appId, global, path, mask, withSubfolders, returnFolders);

        [HttpGet]
        public AssetEditInfo Asset(
            [FromQuery] int appId,
            [FromQuery] int templateId = 0,
            [FromQuery] string path = null, // identifier is always one of these two
            [FromQuery] bool global = false
        ) => Real.Asset(appId, templateId, path, global);

        [HttpPost]
        public bool Create(
            [FromQuery] int appId,
            [FromQuery] string path,
            [FromQuery] bool global,
            [FromQuery] string templateKey
        ) => Real.Create(appId, path, global, templateKey);

        [HttpPost]
        public bool Asset(
            [FromQuery] int appId,
            [FromBody] AssetEditInfo template,
            [FromQuery] int templateId = 0,
            [FromQuery] string path = null, // identifier is either template Id or path
                                            // todo w/SPM - global never seems to be used - must check why and if we remove or add to UI
            [FromQuery] bool global = false
        ) => Real.Asset(appId, template, templateId, path, global);

        [HttpGet]
        public TemplatesDto GetTemplates(string purpose = null, string type = null) => Real.GetTemplates(purpose, type);

        [HttpGet]
        public TemplatePreviewDto Preview(int appId, string path, string templateKey, bool global = false)
            => Real.Preview(appId, path, templateKey, global);

        [HttpGet]
        public AllFilesDto AppFiles(int appId, [FromQuery] string path = null, [FromQuery] string mask = null) => Real.AppFiles(appId, path, mask);

    }
}