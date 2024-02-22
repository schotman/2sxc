﻿using ToSic.Lib.Helpers;
using ToSic.Sxc.Data;
using ToSic.Sxc.DataSources;

namespace ToSic.Sxc.Code.Internal.CodeRunHelpers;

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class TypedCode16Helper(
    ICodeApiService codeApiSvc,
    IBlockInstance data,
    IDictionary<string, object> myModelData,
    bool isRazor,
    string codeFileName)
    : CodeHelperXxBase(codeApiSvc, isRazor, codeFileName, SxcLogName + ".TCd16H")
{
    public bool DefaultStrict = true;

    //protected readonly IDynamicCodeRoot CodeRoot;
    //protected readonly bool IsRazor;
    //protected readonly string CodeFileName;
    internal ContextData Data { get; } = data as ContextData;

    //CodeRoot = codeRoot;
    //IsRazor = isRazor;
    //CodeFileName = codeFileName;
    //this.LinkLog(codeRoot.Log);

    public ITypedItem MyItem => _myItem.Get(() => CodeRoot._Cdf.AsItem(Data.MyItem, propsRequired: DefaultStrict));
    private readonly GetOnce<ITypedItem> _myItem = new();

    public IEnumerable<ITypedItem> MyItems => _myItems.Get(() => CodeRoot._Cdf.AsItems(Data.MyItem, propsRequired: DefaultStrict));
    private readonly GetOnce<IEnumerable<ITypedItem>> _myItems = new();

    public ITypedItem MyHeader => _myHeader.Get(() => CodeRoot._Cdf.AsItem(Data.MyHeader, propsRequired: DefaultStrict));
    private readonly GetOnce<ITypedItem> _myHeader = new();

    public ITypedModel MyModel => _myModel.Get(() => new TypedModel(myModelData, CodeRoot, IsRazor, CodeFileName));
    private readonly GetOnce<ITypedModel> _myModel = new();


    public ITypedStack AllResources => (CodeRoot as CodeApiService)?.AllResources;

    public ITypedStack AllSettings => (CodeRoot as CodeApiService)?.AllSettings;

    //public IDevTools DevTools => _devTools.Get(() => new DevTools(IsRazor, CodeFileName, Log));
    //private readonly GetOnce<IDevTools> _devTools = new GetOnce<IDevTools>();
}