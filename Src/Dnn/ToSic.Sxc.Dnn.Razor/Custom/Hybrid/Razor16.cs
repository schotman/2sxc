﻿using System;
using System.Collections.Generic;
using System.Linq;
using Custom.Hybrid.Advanced;
using ToSic.Eav;
using ToSic.Eav.Code.InfoSystem;
using ToSic.Eav.Plumbing;
using ToSic.Lib.Documentation;
using ToSic.Lib.Helpers;
using ToSic.Sxc.Apps;
using ToSic.Sxc.Code;
using ToSic.Sxc.Data;
using ToSic.Sxc.Services;
using static System.StringComparer;

// ReSharper disable once CheckNamespace
namespace Custom.Hybrid
{
    /// <summary>
    /// Base class for v14 Dynamic Razor files.
    /// Will provide the <see cref="ServiceKit14"/> on property `Kit`.
    /// This contains all the popular services used in v14, so that your code can be lighter. 
    /// </summary>
    /// <remarks>
    /// Important: The property `Convert` which exited on Razor12 was removed. use `Kit.Convert` instead.
    /// </remarks>
    [WorkInProgressApi("WIP 16.02 - not final")]
    public abstract class Razor16: Razor14<dynamic, ServiceKit14>, IDynamicCode16
    {
        #region Killed Properties from base class

        [PrivateApi("Hide as it's nothing that should be used")]
        public new object Content => throw new NotSupportedException($"{nameof(Content)} isn't supported in v16 typed. Use Data.MyContent instead.");

        [PrivateApi("Hide as it's nothing that should be used")]
        public new object Header => throw new NotSupportedException($"{nameof(Header)} isn't supported in v16 typed. Use Data.MyHeader instead.");

        #endregion

        #region Killed DynamicModel and new TypedModel

        [PrivateApi("Hide as it's nothing that should be used")]
        public new object DynamicModel => throw new NotSupportedException($"{nameof(Header)} isn't supported in v16 typed. Use TypedModel instead.");

        #endregion

        #region Temporary v16 objects which must get removed again

        private CodeInfoService CcS => _ccs.Get(GetService<CodeInfoService>);
        private readonly GetOnce<CodeInfoService> _ccs = new GetOnce<CodeInfoService>();

        [PrivateApi]
        public new ITypedStack Settings => CcS.GetAndWarn(DynamicCode16Warnings.AvoidSettingsResources, _DynCodeRoot.Settings);

        [PrivateApi]
        public new ITypedStack Resources => CcS.GetAndWarn(DynamicCode16Warnings.AvoidSettingsResources, _DynCodeRoot.Resources);



        #endregion

        #region New App, Settings, Resources

        /// <inheritdoc />
        public new IAppTyped App => (IAppTyped)base.App;



        /// <inheritdoc />
        public ITypedStack SettingsStack => _DynCodeRoot.Resources;

        /// <inheritdoc />
        public ITypedStack ResourcesStack => _DynCodeRoot.Resources;

        #endregion

        #region My... Stuff

        private TypedCode16Helper CodeHelper => _codeHelper ?? (_codeHelper = CreateCodeHelper());
        private TypedCode16Helper _codeHelper;

        private TypedCode16Helper CreateCodeHelper()
        {
            var myModelData = _overridePageData?.ObjectToDictionaryInvariant()
                      ?? PageData?
                          .Where(pair => pair.Key is string)
                          .ToDictionary(pair => pair.Key.ToString(), pair => pair.Value, InvariantCultureIgnoreCase);

            return new TypedCode16Helper(_DynCodeRoot, Data, myModelData, false, Path);
        }

        public ITypedItem MyItem => CodeHelper.MyItem;

        public IEnumerable<ITypedItem> MyItems => CodeHelper.MyItems;

        public ITypedItem MyHeader => CodeHelper.MyHeader;

        #endregion

        #region AsItem(s) / Merge

        /// <inheritdoc />
        public ITypedStack Merge(params object[] items) => _DynCodeRoot.AsC.MergeTyped(items);
        public ITypedStack AsStack(params object[] items) => _DynCodeRoot.AsC.MergeTyped(items);

        /// <inheritdoc />
        public ITypedItem AsItem(object target, string noParamOrder = Parameters.Protector)
            => _DynCodeRoot.AsC.AsItem(target);

        /// <inheritdoc />
        public IEnumerable<ITypedItem> AsItems(object list, string noParamOrder = Parameters.Protector)
            => _DynCodeRoot.AsC.AsItems(list);

        #endregion


        /// <summary>
        /// Convert a json ... TODO - different from AsTyped(IEntity...)
        /// </summary>
        /// <param name="json"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public ITypedRead Read(string json, string fallback = default) => _DynCodeRoot.AsC.AsDynamicFromJson(json, fallback);


        #region MyModel

        // TODO: MOVE TO V16
        [PrivateApi("WIP 16.02 - to be removed")]
        public ITypedModel TypedModel => CcS.GetAndWarn(DynamicCode16Warnings.NoTypedModel, MyModel);

        [PrivateApi("WIP v16.02")]
        public ITypedModel MyModel => CodeHelper.MyModel;
        //    _typedModel.Get(() =>
        //{
        //    var dic = _overridePageData?.ObjectToDictionary()
        //              ?? PageData?
        //                  .Where(pair => pair.Key is string)
        //                  .ToDictionary(pair => pair.Key.ToString(), pair => pair.Value, InvariantCultureIgnoreCase);

        //    //if (_overridePageData != null)
        //    //    return new TypedModel(_overridePageData.ObjectToDictionary(), _DynCodeRoot, Path);

        //    //var stringDic = PageData?
        //    //    .Where(pair => pair.Key is string)
        //    //    .ToDictionary(pair => pair.Key.ToString(), pair => pair.Value, InvariantCultureIgnoreCase);
        //    return new TypedModel(dic, _DynCodeRoot, Path);
        //});
        //private readonly GetOnce<ITypedModel> _typedModel = new GetOnce<ITypedModel>();


        #endregion

    }


}
