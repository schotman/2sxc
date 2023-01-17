﻿using System.Web;
using ToSic.Lib.DI;
using ToSic.Lib.Logging;
using ToSic.Sxc.Blocks;
using ToSic.Sxc.Context;
using ToSic.Sxc.Dnn.Web;
using Page = System.Web.UI.Page;

namespace ToSic.Sxc.Dnn.Services
{
    public class DnnRenderService : RenderService
    {
        private readonly ILazySvc<DnnPageChanges> _dnnPageChanges;
        private readonly ILazySvc<DnnClientResources> _dnnClientResources;
        private readonly Generator<IContextOfBlock> _context;

        public DnnRenderService(
            Dependencies dependencies,
            ILazySvc<DnnPageChanges> dnnPageChanges,
            ILazySvc<DnnClientResources> dnnClientResources,
            Generator<IContextOfBlock> context
        ) : base(dependencies)
        {
            ConnectServices(
                _dnnPageChanges = dnnPageChanges,
                _dnnClientResources = dnnClientResources,
                _context = context
            );
        }

        public override IRenderResult Module(int pageId, int moduleId) => Log.Func($"{nameof(pageId)}: {pageId}, {nameof(moduleId)}: {moduleId}", () =>
        {
            var result = base.Module(pageId, moduleId);

            // this code should be executed in PreRender of page (ensure when calling) or it is too late
            if (HttpContext.Current?.Handler is Page dnnHandler) // detect if we are on the page
                if (_context.New().Module.BlockIdentifier == null) // find if is in module (because in module it's already handled)
                    DnnPageProcess(dnnHandler, result);

            return (result, "ok");
        });

        private void DnnPageProcess(Page dnnPage, IRenderResult result)
        {
            _dnnPageChanges.Value.Apply(dnnPage, result);
            _dnnClientResources.Value.Init(dnnPage, null, null).AddEverything(result.Features);
        }
    }
}
