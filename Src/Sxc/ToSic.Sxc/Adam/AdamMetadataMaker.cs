﻿using System;
using ToSic.Eav.Context;
using ToSic.Eav.Metadata;
using ToSic.Lib.DI;
using ToSic.Lib.Helpers;
using ToSic.Sxc.Data;

namespace ToSic.Sxc.Adam
{
    /// <summary>
    /// Helpers to get the metadata for ADAM items
    /// </summary>
    public class AdamMetadataMaker
    {
        public AdamMetadataMaker(Generator<DynamicEntity.MyServices> deGenerator)
        {
            _deGenerator = deGenerator;
        }

        private readonly Generator<DynamicEntity.MyServices> _deGenerator;

        ///// <summary>
        ///// Find the first metadata entity for this file/folder
        ///// </summary>
        ///// <param name="app">the app which manages the metadata</param>
        ///// <param name="mdId"></param>
        ///// <returns></returns>
        //internal IEnumerable<IEntity> GetMetadata(AppRuntime app, ITarget mdId)
        //    => app.Metadata.Get(mdId.TargetType, mdId.KeyString);

        /// <summary>
        /// Get the first metadata entity of an item - or return a fake one instead
        /// </summary>
        internal IMetadata GetDynamic(AdamManager manager, string key, string title, Action<IMetadataOf> mdInit = null)
        {
            var mdOf = new MetadataOf<string>((int)TargetTypes.CmsItem, key, title, null, manager.AppRuntime.AppState);
            mdInit?.Invoke(mdOf);
            return new Metadata(mdOf, null, DynamicEntityDependencies(manager));
        }

        private DynamicEntity.MyServices DynamicEntityDependencies(AdamManager manager) =>
            _myDeps.Get(() => manager.DynamicEntityServices ?? _deGenerator.New()
                .Init(null, (manager.AppContext?.Site).SafeLanguagePriorityCodes(), manager.Log,
                    manager.CompatibilityLevel, null, () => manager));

        private readonly GetOnce<DynamicEntity.MyServices> _myDeps = new GetOnce<DynamicEntity.MyServices>();


    }
}
