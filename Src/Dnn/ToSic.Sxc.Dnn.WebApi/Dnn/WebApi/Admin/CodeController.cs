﻿using System.Collections.Generic;
using System.Web.Http;
using ToSic.Sxc.WebApi;
using RealController = ToSic.Sxc.WebApi.Admin.CodeControllerReal;

namespace ToSic.Sxc.Dnn.WebApi.Admin;

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class CodeController : SxcApiControllerBase
{
    public CodeController() : base(RealController.LogSuffix) { }
    private RealController Real => SysHlp.GetService<RealController>();

    [HttpGet]
    public IEnumerable<RealController.HelpItem> InlineHelp(string language) => Real.InlineHelp(language);
}