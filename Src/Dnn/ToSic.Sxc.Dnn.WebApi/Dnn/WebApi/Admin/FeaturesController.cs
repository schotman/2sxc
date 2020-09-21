﻿using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using DotNetNuke.Application;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using ToSic.Eav.Configuration;
using ToSic.Eav.WebApi.PublicApi;
using ToSic.Sxc.WebApi;
using ToSic.Sxc.WebApi.Features;

namespace ToSic.Sxc.Dnn.WebApi.Admin
{
    [SupportedModules("2sxc,2sxc-app")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public class FeatureController : SxcApiControllerBase, IFeatureController
    {
        /// <summary>
        /// Used to be GET System/Features
        /// </summary>
        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Admin)]
        public IEnumerable<Feature> List(bool reload = false) => 
            Eav.Factory.Resolve<FeaturesBackend>().Init(Log).GetAll(reload);

        /// <summary>
        /// Used to be GET System/ManageFeaturesUrl
        /// </summary>
        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Host)]
        public string RemoteManageUrl()
        {
            //if (!UserInfo.IsSuperUser) return "error: user needs SuperUser permissions";

            return "//gettingstarted.2sxc.org/router.aspx?"
                   + $"DnnVersion={DotNetNukeContext.Current.Application.Version.ToString(4)}"
                   + $"&2SexyContentVersion={Settings.ModuleVersion}"
                   + $"&fp={HttpUtility.UrlEncode(Fingerprint.System)}"
                   + $"&DnnGuid={DotNetNuke.Entities.Host.Host.GUID}"
                   + $"&ModuleId={Request.FindModuleInfo().ModuleID}" // needed for callback later on
                   + "&destination=features";
        }

        /// <summary>
        /// Used to be GET System/SaveFeatures
        /// </summary>
        [HttpPost]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Host)]
        public bool Save([FromBody] FeaturesDto featuresManagementResponse) => 
            Eav.Factory.Resolve<FeaturesBackend>().Init(Log).SaveFeatures(featuresManagementResponse);
    }
}