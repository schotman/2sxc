﻿using ToSic.Eav.Documentation;
using ToSic.Sxc.Apps;
using ToSic.Sxc.Blocks;

namespace ToSic.Sxc.Code
{
    public partial class DynamicCodeRoot
    {
        [PrivateApi]
        public void AttachAppAndInitLink(IApp app)
        {
            App = app;
            Link.Init(Block?.Context, App, Log);
        }

        [PrivateApi]
        public int CompatibilityLevel { get; private set; }

        [PrivateApi] public IBlock Block { get; private set; }

    }
}
