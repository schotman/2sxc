﻿using ToSic.Lib.Documentation;
using ToSic.Sxc.Apps;
using ToSic.Sxc.Blocks;
using ToSic.Sxc.Data;
using CodeDataFactory = ToSic.Sxc.Data.Internal.CodeDataFactory;

namespace ToSic.Sxc.Code;

/// <summary>
/// This is the same as IDynamicCode, but the root object. 
/// We create another interface to ensure we don't accidentally pass around a sub-object where the root is really needed.
/// </summary>
[PrivateApi]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public interface IDynamicCodeRoot : IDynamicCode12
{
    [PrivateApi("WIP")] IBlock Block { get; }

    [PrivateApi] void AttachApp(IApp app);


    new IDynamicStack Resources { get; }
    new IDynamicStack Settings { get; }

    #region AsConverter (internal)

    [PrivateApi("internal use only")]
    CodeDataFactory Cdf { get; }

    #endregion
}