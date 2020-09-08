﻿using System;
using System.Linq;
using ToSic.Eav;
using ToSic.Eav.Apps;
using ToSic.Eav.Documentation;
using ToSic.Eav.Logging;
using ToSic.Eav.Run;
using ToSic.Sxc.Blocks;
using IApp = ToSic.Sxc.Apps.IApp;

namespace ToSic.Sxc.Adam
{
    /// <summary>
    /// The app-context of ADAM
    /// In charge of managing assets inside this app
    /// </summary>
    public class AdamAppContext: HasLog, IContextAdamMaybe, ICompatibilityLevel
    {
        /// <summary>
        /// the app is only used to get folder / guid etc.
        /// don't use it to access data! as the data should never have to be initialized for this to work
        /// always use the AppRuntime instead
        /// </summary>
        private readonly IApp _app;
        public readonly AppRuntime AppRuntime;
        public readonly ITenant Tenant;
        public readonly IBlock Block;
        public readonly IAdamFileSystem AdamFs;
        
        public AdamAppContext(ITenant tenant, IApp app, IBlock block, int compatibility, ILog parentLog) : base("Adm.ApCntx", parentLog, "starting")
        {
            Tenant = tenant;
            _app = app;
            Block = block;
            AppRuntime = new AppRuntime(app, block?.EditAllowed ?? false, null);
            CompatibilityLevel = compatibility;
            AdamFs = Factory.Resolve<IAdamFileSystem>().Init(this);
        }

        /// <summary>
        /// Path to the app assets
        /// </summary>
        public string Path => _path ?? (_path = Configuration.AppReplacementMap(_app)
                                  .ReplaceInsensitive(Configuration.AdamAppRootFolder));
        private string _path;


        /// <summary>
        /// Root folder object of the app assets
        /// </summary>
        public Folder RootFolder => Folder(Path, true);

        #region basic, generic folder commands -- all internal

        /// <summary>
        /// Verify that a path exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal bool Exists(string path) => AdamFs.FolderExists(path);

        /// <summary>
        /// Create a path (folder)
        /// </summary>
        /// <param name="path"></param>
        internal void Add(string path) => AdamFs.AddFolder(path);


        internal Folder Folder(string path, bool autoCreate)
        {
            // create all folders to ensure they exist. Must do one-by-one because the environment must have it in the catalog
            var pathParts = path.Split('/');
            var pathToCheck = "";
            foreach (var part in pathParts.Where(p => !string.IsNullOrEmpty(p)))
            {
                pathToCheck += part + "/";
                if (Exists(pathToCheck)) continue;
                if (autoCreate)
                    Add(pathToCheck);
                else
                    throw new Exception("subfolder " + pathToCheck + "not found");
            }

            return Folder(path);
        }


        internal Folder Folder(string path) => AdamFs.Get(path);


        #endregion

        public Export Export => new Export(this);
        [PrivateApi]
        public int CompatibilityLevel { get; }
    }
}