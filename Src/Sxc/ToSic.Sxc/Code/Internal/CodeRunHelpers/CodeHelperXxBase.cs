﻿using ToSic.Lib.Helpers;
using ToSic.Lib.Services;

namespace ToSic.Sxc.Code.Internal.CodeRunHelpers;

[PrivateApi]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public abstract class CodeHelperXxBase(CodeHelperSpecs helperSpecs, string logName)
    : HelperBase(helperSpecs.CodeApiSvc.Log, logName)
{
    // this.LinkLog(helperSpecs.CodeApiSvc.Log);

    protected ICodeApiService CodeRoot => Specs.CodeApiSvc;

    protected CodeHelperSpecs Specs { get; } = helperSpecs;


    public IDevTools DevTools => _devTools.Get(() => new DevTools(Specs, Log));
    private readonly GetOnce<IDevTools> _devTools = new();

}