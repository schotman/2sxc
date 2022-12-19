﻿using ToSic.Eav.Plumbing;
using ToSic.Lib.Helper;
using ToSic.Sxc.Web;
using static ToSic.Sxc.Edit.Toolbar.ToolbarRuleOps;

namespace ToSic.Sxc.Edit.Toolbar
{
    /// <summary>
    /// A toolbar rule for a specific target
    /// </summary>
    public abstract class ToolbarRuleTargeted: ToolbarRule
    {
        protected ToolbarRuleTargeted(
            object target, 
            string command, 
            string ui = null, 
            string parameters = null, 
            char? operation = null,
            ToolbarContext context = null,
            ToolbarButtonDecoratorHelper decoHelper = null
        ) : base(command, ui, parameters: parameters, operation: operation, operationCode: target as string, context: context)
        {
            Target = target;
            DecoHelper = decoHelper;

            var operationCode = target as string;
            // Special case, if target is "-" or "remove" etc.
            if (operationCode.HasValue())
            {
                var targetCouldBeOperation = ToolbarRuleOperation.Pick(operationCode, OprUnknown);
                if (targetCouldBeOperation != (char)OprUnknown)
                {
                    Target = null;
                    Operation = targetCouldBeOperation;
                }
            }
        }

        internal object Target { get; set; }

        protected readonly ToolbarButtonDecoratorHelper DecoHelper;

        public override string GeneratedUiParams()
            => UrlParts.ConnectParameters(UiParams(), base.GeneratedUiParams());


        #region Decorators

        protected virtual string DecoratorTypeName => "";

        protected ToolbarButtonDecorator Decorator => _decorator.Get(() =>
        {
            var decoTypeName = DecoratorTypeName;
            return decoTypeName.HasValue() ? DecoHelper?.GetDecorator(Context, decoTypeName ?? "", Command) : null;
        });
        private readonly GetOnce<ToolbarButtonDecorator> _decorator = new GetOnce<ToolbarButtonDecorator>();
        private string UiParams() => Decorator?.AllRules();

        #endregion
    }
}
