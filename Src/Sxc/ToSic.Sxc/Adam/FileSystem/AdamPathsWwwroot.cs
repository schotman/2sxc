﻿using ToSic.Eav.Internal.Environment;
using ToSic.Lib.Logging;

namespace ToSic.Sxc.Adam
{
    /// <summary>
    /// Basic AdamPaths resolver, assumes that files are in wwwroot/adam for now.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class AdamPathsWwwroot: AdamPathsBase
    {
        public AdamPathsWwwroot(IServerPaths serverPaths) : base(serverPaths, LogScopes.Base)
        {
        }

        /// <summary>
        /// This will just assume that the path - containing 'wwwroot' will not have the 'wwwroot' in the link from outside
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override string Url(string path)
        {
            var original = base.Url(path);
            var url = "/" + original.Replace("wwwroot/", "");
            return url;
        }
    }
}
