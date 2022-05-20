﻿using Oqtane.Models;
using Oqtane.Repository;
using System;
using ToSic.Eav.Logging;
using ToSic.Eav.Plumbing;
using ToSic.Sxc.Blocks;
using ToSic.Sxc.Context;
using ToSic.Sxc.Oqt.Server.Context;
using ToSic.Sxc.Oqt.Server.Integration;
using ToSic.Sxc.Oqt.Shared;

namespace ToSic.Sxc.Oqt.Server.Blocks
{
    public class OqtModuleAndBlockBuilder : ModuleAndBlockBuilder
    {
        public OqtModuleAndBlockBuilder(
            Generator<IModule> moduleGenerator,
            Generator<IContextOfBlock> contextGenerator,
            Generator<BlockFromModule> blockGenerator,
            Generator<IModuleRepository> moduleRepositoryGenerator,
            RequestHelper requestHelper
        ) : base(OqtConstants.OqtLogPrefix)
        {
            _moduleGenerator = moduleGenerator;
            _contextGenerator = contextGenerator;
            _blockGenerator = blockGenerator;
            _moduleRepositoryGenerator = moduleRepositoryGenerator;
            _requestHelper = requestHelper;
        }

        private readonly Generator<IModule> _moduleGenerator;
        private readonly Generator<IContextOfBlock> _contextGenerator;
        private readonly Generator<BlockFromModule> _blockGenerator;
        private readonly Generator<IModuleRepository> _moduleRepositoryGenerator;
        private readonly RequestHelper _requestHelper;
        private ILog ParentLog => Log.Parent ?? Log;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId">not required in Oqtane</param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        protected override IModule GetModuleImplementation(int pageId, int moduleId)
        {
            var oqtModule = (_moduleRepositoryGenerator.New).GetModule(moduleId);
            ThrowIfModuleIsNull(pageId, moduleId, oqtModule);
            var module = ((OqtModule) _moduleGenerator.New).Init(oqtModule, Log);
            return module;
        }

        protected override IBlock GetBlock(IModule module) => GetBlock((module as OqtModule)?.GetContents());

        public override IBlock GetBlock<TPlatformModule>(TPlatformModule module)
        {
            var wrapLog = Log.Fn<IBlock>();
            if (module == null) throw new ArgumentNullException(nameof(module));

            var oqtModule = module switch
            {
                Module oModule => oModule,
                PageModule oPageModule => oPageModule.Module,
                _ => throw new ArgumentException("Given data is not a module")
            };

            Log.A($"Module: {oqtModule.ModuleId}");
            var initializedCtx = InitOqtSiteModuleAndBlockContext(oqtModule);
            var result = _blockGenerator.New.Init(initializedCtx, ParentLog);
            return wrapLog.Return(result, "ok");
        }

        private IContextOfBlock InitOqtSiteModuleAndBlockContext(Module oqtModule)
        {
            var wrapLog = Log.Fn<IContextOfBlock>();
            var context = _contextGenerator.New;
            context.Init(ParentLog);
            //Log.Add($"Will try-swap module info of {oqtModule.ModuleId} into site");
            //((OqtSite)context.Site).TrySwap(oqtModule, ParentLog);
            Log.A("Will init module");
            ((OqtModule) context.Module).Init(oqtModule, ParentLog);
            return wrapLog.Return(InitPageOnly(context));
        }

        private IContextOfBlock InitPageOnly(IContextOfBlock context)
        {
            var wrapLog = Log.Fn<IContextOfBlock>();
            // Collect / assemble page information
            context.Page.Init(_requestHelper.TryGetPageId());
            var url = context.Page.Url;
            return wrapLog.Return(context, url);
        }
    }
}
