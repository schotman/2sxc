﻿using System;
using System.Collections.Generic;
using ToSic.Eav.DataSources;
using ToSic.Eav.Documentation;
using ToSic.Eav.LookUp;
using ToSic.Sxc.Adam;
using ToSic.Sxc.Blocks;
using ToSic.Sxc.Context;
using ToSic.Sxc.Data;
using ToSic.Sxc.DataSources;
using ToSic.Sxc.Web;
using DynamicJacket = ToSic.Sxc.Data.DynamicJacket;
using IApp = ToSic.Sxc.Apps.IApp;
using IEntity = ToSic.Eav.Data.IEntity;


namespace ToSic.Sxc.Hybrid
{
    /// <summary>
    /// The base class for Hybrid Razor-Components in 2sxc 12 <br/>
    /// Provides context infos like the Dnn object, helpers like Edit and much more. <br/>
    /// </summary>
    [PrivateApi]
    public abstract partial class RazorComponent : RazorComponentBase, IHybridRazorComponent
    {

        #region Link, Edit, Dnn, App, Data

        /// <inheritdoc />
        public ILinkHelper Link => _DynCodeRoot.Link;

        /// <inheritdoc />
        public IInPageEditingSystem Edit => _DynCodeRoot.Edit;

        [PrivateApi] public IBlock Block => throw new NotSupportedException("don't use this");

        /// <inheritdoc />
        public TService GetService<TService>() => _DynCodeRoot.GetService<TService>();

        [PrivateApi] public int CompatibilityLevel => _DynCodeRoot.CompatibilityLevel;

        /// <inheritdoc />
        public new IApp App => _DynCodeRoot.App;

        /// <inheritdoc />
        public IBlockDataSource Data => _DynCodeRoot.Data;

        #endregion

        #region AsDynamic in many variations

        /// <inheritdoc/>
        public dynamic AsDynamic(string json, string fallback = DynamicJacket.EmptyJson) => _DynCodeRoot.AsDynamic(json, fallback);

        /// <inheritdoc/>
        public dynamic AsDynamic(IEntity entity) => _DynCodeRoot.AsDynamic(entity);

        /// <inheritdoc/>
        public dynamic AsDynamic(object dynamicEntity) => _DynCodeRoot.AsDynamic(dynamicEntity);


        #endregion

        #region AsEntity
        /// <inheritdoc/>
        public IEntity AsEntity(object dynamicEntity) => _DynCodeRoot.AsEntity(dynamicEntity);
        #endregion

        #region AsList

        /// <inheritdoc />
        public IEnumerable<dynamic> AsList(object list) => _DynCodeRoot.AsList(list);

        #endregion


        #region Data Source Stuff

        /// <inheritdoc/>
        public T CreateSource<T>(IDataSource inSource = null, ILookUpEngine configurationProvider = null)
            where T : IDataSource
            => _DynCodeRoot.CreateSource<T>(inSource, configurationProvider);

        /// <inheritdoc/>
        public T CreateSource<T>(IDataStream inStream) where T : IDataSource
            => _DynCodeRoot.CreateSource<T>(inStream);

        #endregion



        #region Content, Header, etc. and List
        /// <inheritdoc/>
        public dynamic Content => _DynCodeRoot.Content;

        /// <inheritdoc />
        public dynamic Header => _DynCodeRoot.Header;

        #endregion





        #region Adam 

        /// <inheritdoc />
        public IFolder AsAdam(IDynamicEntity entity, string fieldName) => _DynCodeRoot.AsAdam(entity, fieldName);


        /// <inheritdoc />
        public IFolder AsAdam(IEntity entity, string fieldName) => _DynCodeRoot.AsAdam(entity, fieldName);

        #endregion

        #region CmsContext

        [PrivateApi] public ICmsContext CmsContext => _DynCodeRoot.CmsContext;

        #endregion
    }
}
