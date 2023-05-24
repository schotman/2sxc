﻿using System.Linq;
using System.Text.Json.Serialization;
using ToSic.Eav.Data.Shared;
using ToSic.Lib.Logging;
using ToSic.Lib.Services;
using ToSic.Sxc.Blocks;
using ToSic.Sxc.Edit.ClientContextInfo;
using ToSic.Sxc.Services;
using ToSic.Sxc.Web.PageFeatures;

namespace ToSic.Sxc.Web.JsContext
{
    public class JsContextAll : ServiceBase
    {
        public JsContextEnvironment Environment;
        public JsContextUser User;
        public JsContextLanguage Language;
        
        [JsonPropertyName("contentBlockReference")]
        public ContentBlockReferenceDto ContentBlockReference; // todo: still not sure if these should be separate...
        
        [JsonPropertyName("contentBlock")]
        public ContentBlockDto ContentBlock;
        // ReSharper disable once InconsistentNaming
        public ErrorDto error;
        public UiDto Ui;
        public JsApi JsApi;

        public JsContextAll(JsContextLanguage jsLangCtx, IJsApiService jsApiService) : base("Sxc.CliInf")
        {
            ConnectServices(
                _jsLangCtx = jsLangCtx,
                _jsApiService = jsApiService
                );
        }

        private readonly JsContextLanguage _jsLangCtx;
        private readonly IJsApiService _jsApiService;

        public JsContextAll GetJsContext(string systemRootUrl, IBlock block, string errorCode)
        {
            var l = Log.Fn<JsContextAll>();
            var ctx = block.Context;

            Environment = new JsContextEnvironment(systemRootUrl, ctx);
            Language = _jsLangCtx.Init(ctx.Site, block.ZoneId);

            // New in v13 - if the view is from remote, don't allow design
            var view = block.View; // can be null
            var blockCanDesign = (view?.Entity.HasAncestor() ?? false) ? (bool?)false : null;

            User = new JsContextUser(ctx.User, blockCanDesign);

            ContentBlockReference = new ContentBlockReferenceDto(block, ctx.Publishing.Mode);
            ContentBlock = new ContentBlockDto(block);
            var autoToolbar = ctx.UserMayEdit;

            // If auto toolbar is false / not certain, and we have features activated...
            // find out if the Toolbars-Auto is enabled, in which case we should activate them
            if (!autoToolbar && block.BlockFeatureKeys.Any())
            {
                var features = block.Context.PageServiceShared.PageFeatures.GetWithDependents(block.BlockFeatureKeys, Log);
                autoToolbar = features.Contains(BuiltInFeatures.ToolbarsAutoInternal);
            }

            l.A($"{nameof(autoToolbar)}: {autoToolbar}");
            Ui = new UiDto(autoToolbar);
            JsApi = _jsApiService.GetJsApi(pageId: Environment.PageId,
                siteRoot: null,
                rvt: null
            );

            error = new ErrorDto(block, errorCode);
            return l.Return(this);
        }
    }
}
