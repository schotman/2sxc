﻿using System.IO;
using System.Web.Hosting;
using System.Web.WebPages;
using ToSic.Eav.Documentation;
using ToSic.SexyContent.Environment.Dnn7;
using ToSic.Sxc.Code;
using File = System.IO.File;

namespace ToSic.Sxc.Dnn
{
    /// <summary>
    /// The base page type for razor pages
    /// It's the foundation for RazorPage and the old SexyContent page
    /// It only contains internal wiring stuff, so not to be published
    /// </summary>
    [PrivateApi("internal class only!")]
    public abstract class RazorPageBase: WebPageBase, ISharedCodeBuilder
    {
        public IHtmlHelper Html { get; internal set; }

        [PrivateApi]
        protected internal Blocks.ICmsBlock Sexy { get; set; }
        [PrivateApi]
        protected internal DnnAppAndDataHelpers DnnAppAndDataHelpers { get; set; }


        /// <summary>
        /// Override the base class ConfigurePage, and additionally update internal objects so sub-pages work just like the master
        /// </summary>
        /// <param name="parentPage"></param>
        protected override void ConfigurePage(WebPageBase parentPage)
        {
            base.ConfigurePage(parentPage);

            // Child pages need to get their context from the Parent
            Context = parentPage.Context;

            // Return if parent page is not a SexyContentWebPage
            if (!(parentPage is RazorPageBase typedParent)) return;

            // Forward the context
            Html = typedParent.Html;
            Sexy = typedParent.Sexy;
            DnnAppAndDataHelpers = typedParent.DnnAppAndDataHelpers;
        }

        #region Compile Helpers

        public string SharedCodeVirtualRoot { get; set; }

        /// <summary>
        /// Creates instances of the shared pages with the given relative path
        /// </summary>
        /// <returns></returns>
        public dynamic CreateInstance(string virtualPath,
            string dontRelyOnParameterOrder = Eav.Constants.RandomProtectionParameter,
            string name = null,
            string relativePath = null,
            bool throwOnError = true)
        {
            var path = NormalizePath(virtualPath);
            VerifyFileExists(path);
            return path.EndsWith(CodeCompiler.CsFileExtension)
                ? DnnAppAndDataHelpers.CreateInstance(path, dontRelyOnParameterOrder, name, null, throwOnError)
                : CreateInstanceCshtml(path);
        }

        protected dynamic CreateInstanceCshtml(string path)
        {
            var webPage = (RazorPageBase)CreateInstanceFromVirtualPath(path);
            webPage.ConfigurePage(this);
            return webPage;
        }

        protected static void VerifyFileExists(string path)
        {
            if (!File.Exists(HostingEnvironment.MapPath(path)))
                throw new FileNotFoundException("The shared file does not exist.", path);
        }


        #endregion
    }


}