﻿using System;
using ToSic.Eav.Context;
using ToSic.Eav.Data;
using ToSic.Lib.Documentation;
using ToSic.Lib.Helpers;
using ToSic.Sxc.Data;
using ToSic.Sxc.Data.AsConverter;
using ToSic.Sxc.Services.CmsService;

// ReSharper disable ConvertToNullCoalescingCompoundAssignment

namespace ToSic.Sxc.Apps
{
    public partial class App
    {


        #region Dynamic Properties: Configuration, Settings, Resources
        /// <inheritdoc />
        // Create config object. Note that AppConfiguration could be null, then it would use default values
        public AppConfiguration Configuration => _appConfig.Get(() => new AppConfiguration(AppConfiguration, Log));
        private readonly GetOnce<AppConfiguration> _appConfig = new GetOnce<AppConfiguration>();

#if NETFRAMEWORK
        [PrivateApi("obsolete, use the typed accessor instead, only included for old-compatibility")]
        [Obsolete("use the new, typed accessor instead")]
        dynamic SexyContent.Interfaces.IApp.Configuration
        {
            get
            {
                var c = Configuration;
                return c?.Entity != null ? MakeDynProperty(c.Entity) : null;
            }
        }
#endif
        private dynamic MakeDynProperty(IEntity contents)
        {
            var wrapped = CmsEditDecorator.Wrap(contents, false);
            return (_asc ?? _asConverter.Value).AsDynamic(wrapped); // new DynamicEntity(wrapped, DynamicEntityServices);
        }

        internal void AddDynamicEntityServices(AsConverterService asc/*, DynamicEntity.MyServices services*/)
        {
            _asc = asc;
            //_dynEntServices = services;
        }
        private AsConverterService _asc;

        //// TODO: THIS CAN PROBABLY BE IMPROVED
        //// TO GET THE DynamicEntityDependencies from the DynamicCodeRoot which creates the App...? 
        //// ATM it's a bit limited, for example it probably cannot resolve links
        //private DynamicEntity.MyServices DynamicEntityServices
        //    => _dynEntServices
        //       ?? (_dynEntServices = _dynEntSvcLazy.Value.Init(null, Site.SafeLanguagePriorityCodes(), Log));
        //private DynamicEntity.MyServices _dynEntServices;

        /// <inheritdoc />
        public dynamic Settings => AppSettings == null ? null : _settings.Get(() => MakeDynProperty(AppSettings));
        private readonly GetOnce<dynamic> _settings = new GetOnce<dynamic>();

        /// <inheritdoc />
        public dynamic Resources => AppResources == null ? null : _res.Get(() => MakeDynProperty(AppResources));
        private readonly GetOnce<dynamic> _res = new GetOnce<dynamic>();

        #endregion


    }
}