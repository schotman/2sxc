﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Shared;
using System.Collections.Generic;
using ToSic.Eav.WebApi.Admin;
using ToSic.Eav.WebApi.Dto;
using ToSic.Eav.WebApi.Plumbing;
using ToSic.Eav.WebApi.Routing;
using ToSic.Sxc.Oqt.Server.Controllers;
using ToSic.Sxc.WebApi.Adam;
using ToSic.Sxc.WebApi.Admin;

namespace ToSic.Sxc.Oqt.Server.WebApi.Admin
{
    /// <summary>
    /// This one supplies portal-wide (or cross-portal) settings / configuration
    /// </summary>
    //[SupportedModules("2sxc,2sxc-app")]
    //[DnnLogExceptions]
    [Authorize(Roles = RoleNames.Admin)]
    [AutoValidateAntiforgeryToken]

    // Release routes
    [Route(WebApiConstants.ApiRootWithNoLang + $"/{AreaRoutes.Admin}")]
    [Route(WebApiConstants.ApiRootPathOrLang + $"/{AreaRoutes.Admin}")]
    [Route(WebApiConstants.ApiRootPathNdLang + $"/{AreaRoutes.Admin}")]


    public class TypeController : OqtStatefulControllerBase<TypeControllerReal<IActionResult>>, ITypeController<IActionResult>
    {
        public TypeController(): base(TypeControllerReal<IActionResult>.LogSuffix) { }

        [HttpGet]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public IEnumerable<ContentTypeDto> List(int appId, string scope = null, bool withStatistics = false) => Real.List(appId, scope, withStatistics);


        [HttpGet]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public IDictionary<string, string> Scopes(int appId) => Real.Scopes(appId);


        [HttpGet]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public ContentTypeDto Get(int appId, string contentTypeId, string scope = null) => Real.Get(appId, contentTypeId, scope);


        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public bool Delete(int appId, string staticName) => Real.Delete(appId, staticName);


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public bool Save(int appId, [FromBody] Dictionary<string, object> item) => Real.Save(appId, item);


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Host)]
        public bool AddGhost(int appId, string sourceStaticName) => Real.AddGhost(appId, sourceStaticName);


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public void SetTitle(int appId, int contentTypeId, int attributeId) => Real.SetTitle(appId, contentTypeId, attributeId);


        [HttpGet]
        [AllowAnonymous] // will do security check internally
        public IActionResult Json(int appId, string name)
        {
            // Make sure the Scoped ResponseMaker has this controller context
            var responseMaker = (OqtResponseMaker)GetService<ResponseMaker<IActionResult>>();
            responseMaker.Init(this);

            return Real.Json(appId, name);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Admin)]
        public ImportResultDto Import(int zoneId, int appId) => Real.Import(new HttpUploadedFile(Request), zoneId, appId);

    }
}