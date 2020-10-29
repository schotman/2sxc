﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToSic.Eav.WebApi.PublicApi;
using ToSic.Sxc.Oqt.Shared;

namespace ToSic.Sxc.Oqt.Server.Controllers.Admin
{
    /// <inheritdoc cref="IMetadataController" />
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Oqtane.Shared.Constants.AdminRole)]
    [Route(WebApiConstants.WebApiStateRoot + "/admin/[controller]/[action]")]
    public class MetadataController : SxcStatefulControllerBase, IMetadataController
    {
        protected override string HistoryLogName => "Api.Metadata";

        public MetadataController(StatefulControllerDependencies dependencies) : base(dependencies)
        { }

        [HttpGet]
        public IEnumerable<Dictionary<string, object>> Get(int appId, int targetType, string keyType, string key, string contentType)
            => Eav.WebApi.MetadataApi.Get(appId, targetType, keyType, key, contentType);

    }
}