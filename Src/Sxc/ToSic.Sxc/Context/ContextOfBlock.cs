﻿using ToSic.Eav.Apps;
using ToSic.Eav.Context;
using ToSic.Eav.Documentation;
using ToSic.Eav.Logging;
using ToSic.Eav.Plumbing;
using ToSic.Eav.Plumbing.DI;
using ToSic.Sxc.Cms.Publishing;
using ToSic.Sxc.Web.PageService;
// ReSharper disable ConvertToNullCoalescingCompoundAssignment

namespace ToSic.Sxc.Context
{
    [PrivateApi("Internal stuff, not for public use")]
    public class ContextOfBlock: ContextOfApp, IContextOfBlock
    {
        #region Constructor / DI

        public ContextOfBlock(
            IPage page, 
            IModule module, 
            //Lazy<IPagePublishingResolver> publishingResolver,
            LazyInitLog<ServiceSwitcher<IPagePublishingSettings>> publishingResolver,
            PageServiceShared pageServiceShared,
            ContextOfSiteDependencies contextOfSiteDependencies,
            ContextOfAppDependencies appDependencies)
            : base(contextOfSiteDependencies, appDependencies)
        {
            Page = page;
            Module = module;
            PageServiceShared = pageServiceShared;
            //_publishingResolver = publishingResolver;
            _publishingResolver = publishingResolver;
            // special check to prevent duplicate SetLog, because it could be cloned and already initialized
            if (!_publishingResolver.HasInit)
                _publishingResolver.SetLog(Log);
            Log.Rename("Sxc.CtxBlk");
        }
        private readonly LazyInitLog<ServiceSwitcher<IPagePublishingSettings>> _publishingResolver;

        #endregion

        #region Override AppIdentity based on module information

        protected override IAppIdentity AppIdentity
        {
            get
            {
                if (base.AppIdentity != null) return base.AppIdentity;
                var wrapLog = Log.Call<IAppIdentity>();
                var identifier = Module?.BlockIdentifier;
                if (identifier == null) return wrapLog("no mod-block-id", null);
                AppIdentity = identifier;
                return wrapLog(null, base.AppIdentity);
            }
        }

        #endregion

        /// <inheritdoc />
        public IPage Page { get; }

        /// <inheritdoc />
        public IModule Module { get; }

        public PageServiceShared PageServiceShared { get; }

        /// <inheritdoc />
        public BlockPublishingSettings Publishing => _publishing ?? (_publishing = _publishingResolver.Ready.Value.SettingsOfModule(Module?.Id ?? -1));
        private BlockPublishingSettings _publishing;

        /// <inheritdoc />
        public new IContextOfSite Clone(ILog parentLog) => new ContextOfBlock(Page, Module, /*_publishingResolver,*/ _publishingResolver, PageServiceShared, Dependencies, Deps)
            .Init(parentLog);
    }
}
