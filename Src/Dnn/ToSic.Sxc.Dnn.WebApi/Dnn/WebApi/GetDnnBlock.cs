﻿using DotNetNuke.Web.Api;
using System.Linq;
using System.Net.Http;
using ToSic.Eav.DI;
using ToSic.Lib.Logging;
using ToSic.Sxc.Blocks;
using ToSic.Sxc.WebApi;

namespace ToSic.Sxc.Dnn.WebApi
{
    public class DnnGetBlock
    {
 
        private readonly Generator<BlockFromEntity> _blockFromEntity;
        private readonly Generator<IModuleAndBlockBuilder> _moduleAndBlockBuilder;

        public DnnGetBlock(Generator<BlockFromEntity> blockFromEntity, Generator<IModuleAndBlockBuilder> moduleAndBlockBuilder)
        {
            _blockFromEntity = blockFromEntity;
            _moduleAndBlockBuilder = moduleAndBlockBuilder;
        }

        internal IBlock GetCmsBlock(HttpRequestMessage request, ILog log)
        {
            var wrapLog = log.Fn<IBlock>(startTimer: true);

            var moduleInfo = request.FindModuleInfo();

            if (moduleInfo == null)
                return wrapLog.ReturnNull("request ModuleInfo not found");

            var block = _moduleAndBlockBuilder.New.Init(log).GetBlock(moduleInfo, null);

            // check if we need an inner block
            if (request.Headers.Contains(WebApiConstants.HeaderContentBlockId)) { 
                var blockHeaderId = request.Headers.GetValues(WebApiConstants.HeaderContentBlockId).FirstOrDefault();
                int.TryParse(blockHeaderId, out var blockId);
                if (blockId < 0)   // negative id, so it's an inner block
                {
                    log.A($"Inner Content: {blockId}");
                    if (request.Headers.Contains("BlockIds"))
                    {
                        var blockIds = request.Headers.GetValues("BlockIds").FirstOrDefault()?.Split(',');
                        block = FindInnerContentParentBlock(block, blockId, blockIds, log);
                    }
                    block = _blockFromEntity.New.Init(block, blockId, log);
                }
            }

            return wrapLog.ReturnAsOk(block);
        }

        private IBlock FindInnerContentParentBlock(IBlock parent, int contentBlockId, string[] blockIds, ILog log)
        {
            if (blockIds != null && blockIds.Length >= 2)
            {
                foreach (var ids in blockIds) // blockIds is ordered list, from first ancestor till last successor 
                {
                    var parentIds = ids.Split(':');
                    //var parentAppId = int.Parse(parentIds[0]);
                    //var parentContentBlocks = new Guid(parentIds[1]);
                    var id = int.Parse(parentIds[0]);
                    if (!int.TryParse(parentIds[1], out var cbid) || id == cbid || cbid >= 0) continue;
                    if (cbid == contentBlockId) break; // we are done, because block should be parent/ancestor of cbid
                    parent = _blockFromEntity.New.Init(parent, cbid, log);
                }
            }

            return parent;
        }
    }
}
