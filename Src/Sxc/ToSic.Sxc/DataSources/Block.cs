﻿using System;
using System.Collections.Generic;
using ToSic.Eav.Apps;
using ToSic.Eav.DataSource;
using ToSic.Eav.DataSource.Query;
using ToSic.Eav.DataSources;
using ToSic.Lib.Documentation;
using ToSic.Sxc.Data;
#if NETFRAMEWORK
using ToSic.Sxc.Compatibility;
#endif

namespace ToSic.Sxc.DataSources
{
    /// <summary>
    /// The main data source for Blocks. Internally often uses <see cref="CmsBlock"/> to find what it should provide.
    /// It's based on the <see cref="PassThrough"/> data source, because it's just a coordination-wrapper.
    /// </summary>
    [InternalApi_DoNotUse_MayChangeWithoutNotice]
    public class Block : PassThrough, IBlockDataSource
    {
        [PrivateApi("older use case, probably don't publish")]
        public DataPublishing Publish { get; }= new DataPublishing();

        internal void SetOut(Query querySource) => _querySource = querySource;
        private Query _querySource;

        public override IReadOnlyDictionary<string, IDataStream> Out => _querySource?.Out ?? base.Out;

        [PrivateApi("not meant for public use")]
        public Block(MyServices services, IAppStates appStates) : base(services, "Sxc.BlckDs") => _appStates = appStates;
        private readonly IAppStates _appStates;

#if NETFRAMEWORK
#pragma warning disable 618
        [Obsolete("Old property on this data source, should really not be used at all. Must add warning in v13, and remove ca. v15")]
        [PrivateApi]
        public CacheWithGetContentType Cache
        {
            get
            {
                if (_cache != null) return _cache;
                Obsolete.Warning13To15("Data.Cache", "", "https://r.2sxc.org/brc-13-datasource-cache");
                return _cache = new CacheWithGetContentType(_appStates.Get(this));
            }
        }

        [Obsolete]
        private CacheWithGetContentType _cache;
#pragma warning restore 618
#endif
    }
}