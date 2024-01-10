﻿using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using ToSic.Sxc.Backend.Admin;
using ToSic.Sxc.Dnn.WebApi.Logging;
using ToSic.Sxc.WebApi;
using RealController = ToSic.Sxc.Backend.Admin.DialogControllerReal;

namespace ToSic.Sxc.Dnn.WebApi.Admin;

/// <summary>
/// This one supplies portal-wide (or cross-portal) settings / configuration
/// </summary>
[SupportedModules(DnnSupportedModuleNames)]
[DnnLogExceptions]
[DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Admin)]
[ValidateAntiForgeryToken]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class DialogController : SxcApiControllerBase, IDialogController
{


    public DialogController(): base(RealController.LogSuffix) { }

    private RealController Real => SysHlp.GetService<RealController>();

    [HttpGet]
    public DialogContextStandaloneDto Settings(int appId) => Real.Settings(appId);
}