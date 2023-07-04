﻿using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ToSic.Lib.Documentation;
using ToSic.Sxc;
using ToSic.Sxc.Apps;
using ToSic.Sxc.Code;
using ToSic.Sxc.Data;
using ToSic.Sxc.Razor;
using ToSic.Sxc.Services;
using IHasLog = ToSic.Lib.Logging.IHasLog;
using ILog = ToSic.Lib.Logging.ILog;
using static ToSic.Eav.Parameters;
using System.Collections.Generic;
using ToSic.Eav.Data;
using ToSic.Eav.DataSource;
using ToSic.Eav.LookUp;
using ToSic.Sxc.Adam;
using ToSic.Sxc.Code.DevTools;
using ToSic.Sxc.Context;
using ToSic.Sxc.Engines;

// ReSharper disable once CheckNamespace
namespace Custom.Hybrid
{
    [PrivateApi("This will already be documented through the Dnn DLL so shouldn't appear again in the docs")]
    public abstract partial class Razor12<TModel>: Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>, IHasCodeLog, IRazor, IRazor12, ISetDynamicModel
    {
        #region Constructor / DI / SysHelp

        /// <summary>
        /// Constructor - only available for inheritance
        /// </summary>
        [PrivateApi]
        protected Razor12() { }

        [PrivateApi] public int CompatibilityLevel => Constants.CompatibilityLevel12;

        /// <summary>
        /// Special helper to move all Razor logic into a separate class.
        /// For architecture of Composition over Inheritance.
        /// </summary>
        [PrivateApi]
        internal OqtRazorHelper<TModel> SysHlp => _sysHlp ??= new(this, "Oqt.Rzr12"); //.Init(this);
        private OqtRazorHelper<TModel> _sysHlp;

        #endregion

        #region GetService / Logs / DevTools

        /// <inheritdoc cref="IDynamicCode.GetService{TService}" />
        public TService GetService<TService>() => _DynCodeRoot.GetService<TService>();

        /// <inheritdoc cref="IHasCodeLog.Log" />
        public ICodeLog Log => SysHlp.CodeLog;

        [PrivateApi] ILog IHasLog.Log => SysHlp.Log;

        [PrivateApi("Not yet ready")]
        public IDevTools DevTools => _DynCodeRoot.DevTools;

        #endregion

        #region DynCode Root

        [PrivateApi]
        public IDynamicCodeRoot _DynCodeRoot => SysHlp._DynCodeRootMain;

        [PrivateApi]
        public void ConnectToRoot(IDynamicCodeRoot parent) => SysHlp.ConnectToRoot(parent);

        [RazorInject]
        [PrivateApi]
        public new ViewDataDictionary<TModel> ViewData
        {
            get => base.ViewData;
            set => base.ViewData = SysHlp.HandleViewDataInject(value);
        }

        #endregion

        #region Dynamic Model

        public dynamic DynamicModel => SysHlp.DynamicModel;

        void ISetDynamicModel.SetDynamicModel(object data) => SysHlp.SetDynamicModel(data);

        #endregion



        #region CreateInstance

        /// <inheritdoc cref="ICreateInstance.CreateInstancePath"/>
        [PrivateApi]
        // Note: The path for CreateInstance / GetCode - unsure if this is actually used anywhere on this object
        string IGetCodePath.CreateInstancePath
        {
            get => _createInstancePath ?? Path;
            set => _createInstancePath = value;
        }
        private string _createInstancePath;

        /// <inheritdoc cref="ICreateInstance.CreateInstance"/>
        public dynamic CreateInstance(string virtualPath, string noParamOrder = Protector, string name = null, string relativePath = null, bool throwOnError = true)
            => SysHlp.CreateInstance(virtualPath, Path, noParamOrder, name, relativePath, throwOnError);

        #endregion

        #region Content, Header, etc.

        /// <inheritdoc cref="IDynamicCode.Content" />
        public dynamic Content => _DynCodeRoot.Content;

        /// <inheritdoc cref="IDynamicCode.Header" />
        public dynamic Header => _DynCodeRoot.Header;

        #endregion


        #region Link, Edit, App, Data

        /// <inheritdoc cref="IDynamicCode.Link" />
        public ILinkService Link => _DynCodeRoot.Link;

        /// <inheritdoc cref="IDynamicCode.Link" />
        public IEditService Edit => _DynCodeRoot.Edit;

        /// <inheritdoc cref="IDynamicCode.App" />
        public IApp App => _DynCodeRoot.App;

        /// <inheritdoc cref="IDynamicCode.Data" />
        public IContextData Data => _DynCodeRoot.Data;

        #endregion

        #region AsDynamic in many variations + AsList

        /// <inheritdoc cref="IDynamicCode.AsDynamic(string, string)" />
        public dynamic AsDynamic(string json, string fallback = default) => _DynCodeRoot.AsC.AsDynamicFromJson(json, fallback);

        /// <inheritdoc cref="IDynamicCode.AsDynamic(object)" />
        public dynamic AsDynamic(IEntity entity) => _DynCodeRoot.AsC.AsDynamic(entity);

        /// <inheritdoc cref="IDynamicCode.AsDynamic(string, string)" />
        public dynamic AsDynamic(object dynamicEntity) => _DynCodeRoot.AsC.AsDynamicInternal(dynamicEntity);

        /// <inheritdoc cref="IDynamicCode12.AsDynamic(object[])" />
        public dynamic AsDynamic(params object[] entities) => _DynCodeRoot.AsC.MergeDynamic(entities);

        /// <inheritdoc cref="IDynamicCode.AsList" />
        public IEnumerable<dynamic> AsList(object list) => _DynCodeRoot.AsC.AsDynamicList(list);

        #endregion

        #region AsEntity

        /// <inheritdoc cref="IDynamicCode.AsEntity" />
        public IEntity AsEntity(object dynamicEntity) => _DynCodeRoot.AsC.AsEntity(dynamicEntity);

        #endregion

        #region CreateSource

        /// <inheritdoc cref="IDynamicCode.CreateSource{T}(IDataStream)" />
        public T CreateSource<T>(IDataStream source) where T : IDataSource
            => _DynCodeRoot.CreateSource<T>(source);

        /// <inheritdoc cref="IDynamicCode.CreateSource{T}(IDataSource, ILookUpEngine)" />
        public T CreateSource<T>(IDataSource inSource = null, ILookUpEngine configurationProvider = default) where T : IDataSource
            => _DynCodeRoot.CreateSource<T>(inSource, configurationProvider);

        #endregion

        #region Convert-Service - V12 only!

        [PrivateApi] public IConvertService Convert => _DynCodeRoot.Convert;

        #endregion

        #region Adam

        /// <inheritdoc cref="IDynamicCode.AsAdam" />
        public IFolder AsAdam(ICanBeEntity item, string fieldName) => _DynCodeRoot.AsAdam(item, fieldName);

        #endregion

        #region CmsContext 

        /// <inheritdoc cref="IDynamicCode.CmsContext" />
        public ICmsContext CmsContext => _DynCodeRoot.CmsContext;

        /// <inheritdoc cref="IDynamicCode12.Resources" />
        public dynamic Resources => _DynCodeRoot.Resources;

        /// <inheritdoc cref="IDynamicCode12.Settings" />
        public dynamic Settings => _DynCodeRoot.Settings;

        #endregion

    }
}
