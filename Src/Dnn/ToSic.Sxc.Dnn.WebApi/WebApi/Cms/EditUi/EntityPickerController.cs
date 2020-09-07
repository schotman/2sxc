﻿using System.Collections.Generic;
using System.Web.Http;
using DotNetNuke.Web.Api;
using ToSic.Eav.WebApi.Dto;
using ToSic.Eav.WebApi.PublicApi;

namespace ToSic.Sxc.WebApi.Cms
{
    [SupportedModules("2sxc,2sxc-app")]
    [ValidateAntiForgeryToken]
    public class EntityPickerController : SxcApiControllerBase, IEntityPickerController
    {
        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
 		//[DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public IEnumerable<EntityForPickerDto> GetAvailableEntities([FromUri]int appId, [FromBody] string[] items, [FromUri] string contentTypeName = null, [FromUri] int? dimensionId = null)
        {
            return new EntityPickerBackend().Init(Log).GetAvailableEntities(GetContext(), appId, items, contentTypeName, dimensionId);
            //// do security check
            //var permCheck = string.IsNullOrEmpty(contentTypeName) 
            //    ? new MultiPermissionsApp().Init(GetContext(), GetApp(appId), Log)
            //    : new MultiPermissionsTypes().Init(GetContext(), GetApp(appId), contentTypeName, Log);
            //if(!permCheck.EnsureAll(GrantSets.ReadSomething, out var error))
            //    throw HttpException.PermissionDenied(error);

            //// maybe in the future, ATM not relevant
            //var withDrafts = permCheck.EnsureAny(GrantSets.ReadDraft);

            //return new Eav.WebApi.EntityPickerApi(Log)
            //    .GetAvailableEntities(appId, items, contentTypeName, withDrafts, dimensionId);
        }
    }
}