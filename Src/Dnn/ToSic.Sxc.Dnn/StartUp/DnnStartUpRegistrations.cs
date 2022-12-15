﻿using ToSic.Eav.Configuration;
using ToSic.Lib.Logging;
using ToSic.Eav.Run;

namespace ToSic.Sxc.Dnn.StartUp
{
    public class DnnStartUpRegistrations: HasLog, IStartUpRegistrations
    {
        public string NameId => Log.NameId;

        public DnnStartUpRegistrations(FeaturesCatalog featuresCatalog): base($"{DnnConstants.LogName}.SUpReg")
        {
            _featuresCatalog = featuresCatalog;
        }
        private readonly FeaturesCatalog _featuresCatalog;

        /// <summary>
        /// Register Dnn features before loading
        /// </summary>
        public void Register() => Configuration.Features.BuiltInFeatures.Register(_featuresCatalog);

    }
}