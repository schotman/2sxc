﻿using DotNetNuke.Common;
using ToSic.Lib.Documentation;
using ToSic.Lib.Logging;
using ToSic.Lib.Services;
using ToSic.Razor.Blade;
using ToSic.Sxc.Edit;

namespace ToSic.Sxc.Dnn.Web
{
    [PrivateApi]
    public class DnnJsApiHeader: HelperBase
    {
        public DnnJsApiHeader(ILog parentLog = null) : base(parentLog, "Dnn.JsApiH")
        {
        }

        public bool AddHeaders() => Log.Func(() =>
        {
            // ensure we only do this once
            if (MarkAddedAndReturnIfAlreadyDone()) return (false, "already");

            var json = DnnJsApi.GetJsApiJson();
            if (json == null) return (false, "no path");

#pragma warning disable CS0618
            HtmlPage.AddMeta(InpageCms.MetaName, json);
#pragma warning restore CS0618
            return (true, "added");
        });
  
        private const string KeyToMarkAdded = "2sxcApiHeadersAdded";

        private static bool MarkAddedAndReturnIfAlreadyDone()
        {
            var alreadyAdded = HttpContextSource.Current.Items.Contains(KeyToMarkAdded);
            HttpContextSource.Current.Items[KeyToMarkAdded] = true;
            return alreadyAdded;
        }
    }
}
