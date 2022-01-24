﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using ToSic.Eav.Context;
using ToSic.Eav.Logging;
using ToSic.Eav.Plumbing;
using ToSic.Sxc.Blocks;
using ToSic.Sxc.Engines;

namespace ToSic.Sxc.Apps.Assets
{
    public class AssetEditor : HasLog
    {
        #region Constructor / DI

        private AssetEditInfo EditInfo { get; set; }

        private readonly Lazy<CmsRuntime> _cmsRuntimeLazy;
        private readonly IUser _user;
        private CmsRuntime _cmsRuntime;
        private IApp _app;

        public AssetEditor(Lazy<CmsRuntime> cmsRuntimeLazy, IUser user) : base("Sxc.AstEdt")
        {
            _cmsRuntimeLazy = cmsRuntimeLazy;
            _user = user;
        }

        // TODO: REMOVE THIS once we release v13 #cleanUp EOY 2021
        public AssetEditor Init(IApp app, int templateId, ILog parentLog)
        {
            InitShared(app, parentLog);
            var view = _cmsRuntime.Views.Get(templateId);
            var t = new AssetEditInfo(_app.AppId, _app.Name, view.Path, view.IsShared);
            EditInfo = AddViewDetailsAndTypes(t, view);

            //EditInfo = TemplateAssetsInfo(template);
            return this;
        }

        public AssetEditor Init(IApp app, string path, bool global, int viewId, ILog parentLog)
        {
            InitShared(app, parentLog);
            EditInfo = new AssetEditInfo(_app.AppId, _app.Name, path, global);
            if (viewId == 0) return this;

            var view = _cmsRuntime.Views.Get(viewId);
            AddViewDetailsAndTypes(EditInfo, view);
            return this;
        }


        private void InitShared(IApp app, ILog parentLog)
        {
            Log.LinkTo(parentLog);
            _app = app;

            // todo: 2dm Views - see if we can get logger to flow
            _cmsRuntime = _cmsRuntimeLazy.Value.Init(app, true, Log);
        }

        #endregion

        public const string CshtmlPrefix = "_";

        public AssetEditInfo EditInfoWithSource
        {
            get
            {
                EditInfo.Code = Source; // do this later, because it relies on the edit-info to exist
                return EditInfo;
            }
        }

        /// <summary>
        /// Check permissions and if not successful, give detailed explanation
        /// </summary>
        public void EnsureUserMayEditAssetOrThrow(string fullPath = null)
        {
            // check super user permissions - then all is allowed
            if (_user.IsSuperUser)// _userIsSuperUser)
                return;

            // ensure current user is admin - this is the minimum of not super-user
            if (!_user.IsAdmin) // _userIsAdmin)
                throw new AccessViolationException("current user may not edit templates, requires admin rights");

            // if not super user, check if razor (not allowed; super user only)
            if (!EditInfo.IsSafe)
                throw new AccessViolationException("current user may not edit razor templates - requires super user");

            // if not super user, check if cross-portal storage (not allowed; super user only)
            if (EditInfo.IsShared)
                throw new AccessViolationException(
                    "current user may not edit templates in central storage - requires super user");

            // optionally check if the file is really in the portal
            if (fullPath == null) return;

            var path = new FileInfo(fullPath);
            if (path.Directory == null)
                throw new AccessViolationException("path is null");

            if (path.Directory.FullName.IndexOf(_app.PhysicalPath, StringComparison.InvariantCultureIgnoreCase) != 0)
                throw new AccessViolationException("current user may not edit files outside of the app-scope");
        }

        //private AssetEditInfo TemplateAssetsInfo(IView view)
        //{
        //    var t = new AssetEditInfo(_app.AppId, _app.Name, view.Path, view.IsShared);
        //    return AddViewDetailsAndTypes(t, view);
        //}

        private static AssetEditInfo AddViewDetailsAndTypes(AssetEditInfo t, IView view)
        {
            // Template specific properties, not really available in other files
            t.Type = view.Type;
            t.Name = view.Name;
            t.HasList = view.UseForList;
            t.TypeContent = view.ContentType;
            t.TypeContentPresentation = view.PresentationType;
            t.TypeList = view.HeaderType;
            t.TypeListPresentation = view.HeaderPresentationType;
            return t;
        }

        public string InternalPath => NormalizePath(Path.Combine(
            _cmsRuntime.ServiceProvider.Build<AppPathHelpers>().Init(_app, Log)
                .AppPathRoot(EditInfo.IsShared, PathTypes.PhysFull), EditInfo.FileName));

        private static string NormalizePath(string path) => Path.GetFullPath(new Uri(path).LocalPath);

        /// <summary>
        /// Read / Write the source code of the template file
        /// </summary>
        public string Source
        {
            get
            {
                EnsureUserMayEditAssetOrThrow(InternalPath);
                if (File.Exists(InternalPath))
                    return File.ReadAllText(InternalPath);

                throw new FileNotFoundException("could not find file"
                                                + (_user.IsSuperUser // _userIsSuperUser
                                                    ? " for superuser - file tried '" + InternalPath + "'"
                                                    : "")
                );
            }
            set
            {
                EnsureUserMayEditAssetOrThrow(InternalPath);

                if (File.Exists(InternalPath))
                    File.WriteAllText(InternalPath, value);
                else
                    throw new FileNotFoundException("could not find file"
                                                    + (_user.IsSuperUser // _userIsSuperUser
                                                        ? " for superuser - file tried '" + InternalPath + "'"
                                                        : "")
                    );

            }
        }

        public bool Create(string contents)
        {
            // don't create if it already exits
            if (SanitizeFileNameAndCheckIfAssetAlreadyExists()) return false;

            // ensure the web.config exists (usually missing in the global area)
            _cmsRuntime.ServiceProvider.Build<AppPathHelpers>().Init(_app, Log)
                .EnsureTemplateFolderExists(EditInfo.IsShared);

            var absolutePath = InternalPath;

            EnsureFolders(absolutePath);

            // now create the file
            CreateAsset(absolutePath, contents);

            return true;
        }

        private void SanitizeFileName()
        {
            // todo: maybe add some security for special dangerous file names like .cs, etc.?
            EditInfo.FileName = Regex.Replace(EditInfo.FileName, @"[?:\/*""<>|]", "");
        }

        // check if the folder already exists, or create it...
        private static void EnsureFolders(string absolutePath)
        {
            var foundFolder = absolutePath.LastIndexOf("\\", StringComparison.InvariantCulture);
            if (foundFolder > -1)
            {
                var folderPath = absolutePath.Substring(0, foundFolder);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
            }
        }

        private static void CreateAsset(string absolutePath, string contents)
        {
            using (var stream = new StreamWriter(File.Create(absolutePath)))
            {
                stream.Write(contents);
                stream.Flush();
                stream.Close();
            }
        }

        public bool SanitizeFileNameAndCheckIfAssetAlreadyExists()
        {
            SanitizeFileName();

            return File.Exists(InternalPath);
        }
    }
}