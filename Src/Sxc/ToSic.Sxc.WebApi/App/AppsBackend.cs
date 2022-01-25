﻿using System;
using System.Collections.Generic;
using System.Linq;
using ToSic.Eav.Apps;
using ToSic.Eav.Context;
using ToSic.Eav.WebApi;
using ToSic.Eav.WebApi.Dto;
using ToSic.Sxc.Apps;
using ToSic.Sxc.LookUp;
using IApp = ToSic.Sxc.Apps.IApp;

namespace ToSic.Sxc.WebApi.App
{
    public class AppsBackend: WebApiBackendBase<AppsBackend>
    {
        private readonly CmsZones _cmsZones;
        private readonly IContextOfSite _context;

        public AppsBackend(CmsZones cmsZones, IContextOfSite context, IServiceProvider serviceProvider) : base(serviceProvider, "Bck.Apps")
        {
            _cmsZones = cmsZones;
            _context = context;
        }

        public List<AppDto> Apps()
        {
            var cms = _cmsZones.Init(_context.Site.ZoneId, Log);
            var configurationBuilder = GetService<AppConfigDelegate>().Init(Log).Build(_context.UserMayEdit);
            var list = cms.AppsRt.GetApps(_context.Site, configurationBuilder);
            return list.Select(CreateAppDto).ToList();
        }

        private static AppDto CreateAppDto(IApp a) =>
            new AppDto
            {
                Id = a.AppId,
                IsApp = a.AppGuid != Eav.Constants.DefaultAppGuid &&
                        a.AppGuid != Eav.Constants.PrimaryAppGuid, // #SiteApp v13
                Guid = a.AppGuid,
                Name = a.Name,
                Folder = a.Folder,
                AppRoot = a.Path,
                IsHidden = a.Hidden,
                ConfigurationId = a.Configuration?.Id,
                Items = a.Data.List.Count(),
                Thumbnail = a.Thumbnail,
                Version = a.VersionSafe(),
                IsGlobal = a.AppState.IsGlobal(),
                IsInherited = a.AppState.IsInherited(),
            };

        public List<AppDto> GetInheritableApps()
        {
            var cms = _cmsZones.Init(_context.Site.ZoneId, Log);
            var configurationBuilder = GetService<AppConfigDelegate>().Init(Log).Build(_context.UserMayEdit);
            var list = cms.AppsRt.GetInheritableApps(_context.Site, configurationBuilder);
            return list.Select(CreateAppDto).ToList();
        }
    }
}
