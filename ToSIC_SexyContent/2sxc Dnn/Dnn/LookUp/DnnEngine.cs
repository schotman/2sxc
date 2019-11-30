﻿using System.Threading;
using DotNetNuke.Entities.Portals;
using ToSic.Eav.LookUp;

namespace ToSic.Sxc.Dnn.LookUp
{
    /// <summary>
    /// Retrieves the current engine for a specific module. <br/>
    /// Internally it asks DNN for the current Property-Access objects and prepares them for use in EAV.
    /// </summary>
    public class GetDnnEngine : IGetEngine
    {
        /// <inheritdoc />
        public ILookUpEngine GetEngine(int instanceId)
        {
            var providers = new LookUpEngine();
            var portalSettings = PortalSettings.Current;

            if (portalSettings == null) return providers;

            var dnnUsr = portalSettings.UserInfo;
            var dnnCult = Thread.CurrentThread.CurrentCulture;
            var dnn = new DnnTokenReplace(instanceId, portalSettings, dnnUsr);
            var stdSources = dnn.PropertySources;
            foreach (var propertyAccess in stdSources)
                providers.Sources.Add(propertyAccess.Key,
                    new LookUpInDnnPropertyAccess(propertyAccess.Key, propertyAccess.Value, dnnUsr, dnnCult));
            return providers;
        }
    }
}
