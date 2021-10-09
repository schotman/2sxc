﻿using ToSic.Eav.Documentation;
using ToSic.Sxc.Code.DevTools;
using ToSic.Sxc.Web;

namespace ToSic.Sxc.Code
{
    /// <summary>
    /// New base class for v12 Dynamic Code
    /// Adds new properties and methods, and doesn't keep old / legacy APIs
    /// </summary>
    [PublicApi]
    public class DynamicCode12: DynamicCode, IDynamicCode12
    {

        #region Convert-Service

        /// <inheritdoc />
        public IConvertService Convert => _DynCodeRoot.Convert;

        #endregion

        /// <inheritdoc />
        public dynamic Resources => _DynCodeRoot?.Resources;

        /// <inheritdoc />
        public dynamic Settings => _DynCodeRoot?.Settings;

        public IDevTools DevTools => _DynCodeRoot.DevTools;
    }
}
