﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToSic.Eav.Apps;
using ToSic.Lib.DI;
using ToSic.Lib.Documentation;
using ToSic.Lib.Logging;
using ToSic.Lib.Services;
using ToSic.Sxc.Code;
using ToSic.Sxc.Web;

namespace ToSic.Sxc.Edit.Toolbar
{
    public partial class ToolbarBuilder: HybridHtmlString, IEnumerable<string>, IToolbarBuilder, INeedsDynamicCodeRoot
    {

        #region Constructors and Init

        public class MyServices: MyServicesBase
        {
            public MyServices(
                LazySvc<IAppStates> appStatesLazy,
                LazySvc<ToolbarButtonDecoratorHelper> toolbarButtonHelper
            )
            {
                ConnectServices(
                    ToolbarButtonHelper = toolbarButtonHelper,
                    AppStatesLazy = appStatesLazy
                );
            }

            internal readonly LazySvc<IAppStates> AppStatesLazy;
            public LazySvc<ToolbarButtonDecoratorHelper> ToolbarButtonHelper { get; }
        }

        /// <summary>
        /// Public constructor for DI
        /// </summary>
        /// <param name="services"></param>
        public ToolbarBuilder(MyServices services) => Services = services.ConnectServices(Log);
        protected readonly MyServices Services;

        /// <summary>
        /// Clone-constructor
        /// </summary>
        private ToolbarBuilder(ToolbarBuilder parent): this(parent.Services)
        {
            this.LinkLog(parent.Log);
            _currentAppIdentity = parent._currentAppIdentity;
            _codeRoot = parent._codeRoot;
            _configuration = parent._configuration;
            _utils = parent._utils;
            Rules.AddRange(parent.Rules);
        }

        public ILog Log { get; } = new Log(Constants.SxcLogName + ".TlbBld");

        private IAppIdentity _currentAppIdentity;

        public void ConnectToRoot(IDynamicCodeRoot codeRoot)
        {
            if (codeRoot == null) return;
            _codeRoot = codeRoot;
            _currentAppIdentity = codeRoot.App;
            Services.ToolbarButtonHelper.Value.MainAppIdentity = _currentAppIdentity;
        }
        private IDynamicCodeRoot _codeRoot;

        #endregion

        private ToolbarBuilderConfiguration _configuration;

        private ToolbarBuilderUtilities Utils => _utils ?? (_utils = new ToolbarBuilderUtilities());
        private ToolbarBuilderUtilities _utils;

        public List<ToolbarRuleBase> Rules { get; } = new List<ToolbarRuleBase>();


        public IToolbarBuilder Toolbar(
            string toolbarTemplate,
            object target = null,
            object ui = null,
            object parameters = null,
            object prefill = null
        )
        {
            var updated = AddInternal(new ToolbarRuleToolbar(toolbarTemplate, ui: PrepareUi(ui)));
            if (target != null || parameters != null || prefill != null) 
                updated = updated.Parameters(target, parameters: parameters, prefill: prefill);
            return updated;
        }


        private T FindRule<T>() where T : class => Rules.FirstOrDefault(r => r is T) as T;


        #region Enumerators

        [PrivateApi]
        public IEnumerator<string> GetEnumerator()
        {
            var rulesToDeliver = Rules;

            // **Special**
            // Previously standalone toolbars also hovered based on their wrapper DIV.
            // But this isn't actually useful any more - normally hover is done with a non-standalone toolbar.
            // But we cannot change the JS defaults, because that would affect old toolbars
            // So any standalone toolbar created using the tag-builder will automatically add a settings
            // to not-hover by default. 
            // The rule must be added to the top of the list, so that any other settings will take precedence,
            // Including UI rules added to the toolbar itself
            if (_configuration?.Mode == ToolbarHtmlModes.Standalone)
            {
                var standaloneSettings = new ToolbarRuleSettings(show: "always", hover: "none");
                rulesToDeliver = new List<ToolbarRule> { standaloneSettings }.Concat(Rules).ToList();
            }

            return rulesToDeliver.Select(r => r.ToString()).GetEnumerator();
        }

        [PrivateApi]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

    }
}
