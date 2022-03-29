﻿using System;
using ToSic.Sxc.Data;
using ToSic.Sxc.Edit.Toolbar;
using ToSic.Sxc.Web;
using IEntity = ToSic.Eav.Data.IEntity;

namespace ToSic.Sxc.Edit.EditService
{
    public partial class EditService
    {
        private readonly string innerContentAttribute = "data-list-context";

        /// <inheritdoc />
        public IHybridHtmlString Toolbar(
            object target = null,
            string noParamOrder = Eav.Parameters.Protector,
            string actions = null,
            string contentType = null,
            object condition = null,
            object prefill = null,
            object settings = null,
            object toolbar = null)
            => ToolbarInternal(false, target, noParamOrder, actions, contentType, condition, prefill, settings, toolbar);

        /// <inheritdoc/>
        public IHybridHtmlString TagToolbar(
            object target = null,
            string noParamOrder = Eav.Parameters.Protector,
            string actions = null,
            string contentType = null,
            object condition = null,
            object prefill = null,
            object settings = null,
            object toolbar = null)
            => ToolbarInternal(true, target, noParamOrder, actions, contentType, condition, prefill, settings, toolbar);

        private IHybridHtmlString ToolbarInternal(
            bool inTag,
            object target,
            string noParamOrder,
            string actions,
            string contentType,
            object condition,
            object prefill,
            object settings,
            object toolbar)
        {
            var wrapLog = Log.Call<IHybridHtmlString>($"enabled:{Enabled}; inline{inTag}");
            if (!Enabled) return wrapLog("not enabled", null);
            if (!IsConditionOk(condition)) return wrapLog("condition false", null);

            Eav.Parameters.ProtectAgainstMissingParameterNames(noParamOrder, "Toolbar",
                $"{nameof(actions)},{nameof(contentType)},{nameof(condition)},{nameof(prefill)},{nameof(settings)},{nameof(toolbar)}");

            // New in v13: The first parameter can also be a ToolbarBuilder, in which case all other params are ignored
            ItemToolbar itmToolbar;
            if (target is IToolbarBuilder)
            {
                Log.Add("Using new modern Item-Toolbar, will ignore all other parameters.");
                itmToolbar = new ItemToolbar(null, toolbar: target);
            }
            else
            {
                // ensure that internally we always process it as an entity
                var eTarget = target as IEntity ?? (target as IDynamicEntity)?.Entity;
                if (target != null && eTarget == null)
                    Log.Warn("Creating toolbar - it seems the object provided was neither null, IEntity nor DynamicEntity");
                if (toolbar is IToolbarBuilder)
                {
                    Log.Add("Using new modern Item-Toolbar with an entity, will ignore all other parameters.");
                    itmToolbar = new ItemToolbar(eTarget, toolbar: toolbar);
                }
                else
                {
                    Log.Add("Using classic mode, with all parameters.");
                    itmToolbar = new ItemToolbar(eTarget, actions, contentType, prefill: prefill, settings: settings,
                        toolbar: toolbar);
                }
            }

            var result = inTag
                ? Attribute("sxc-toolbar", itmToolbar.ToolbarAttribute())
                : new HybridHtmlString(itmToolbar.Toolbar);
            return wrapLog("ok", result);
        }

        private bool IsConditionOk(object condition)
        {
            var wrapLog = Log.Call<bool>();

            // Null = no condition and certainly not false, say ok
            if (condition == null) return wrapLog("null,true", true);

            // Bool (non-null) and nullable
            if (condition is bool b && b == false) return wrapLog($"{false}", false);
            if (condition as bool? == false) return wrapLog("null false", false);

            // Int are only false if exactly 0
            if (condition is int i && i == 0) return wrapLog("int 0", false);
            if (condition as int? == 0) return wrapLog("int nullable 0", false);

            // String
            if (condition is string s &&
                string.Equals(s, false.ToString(), StringComparison.InvariantCultureIgnoreCase))
                return wrapLog("string false", false);

            // Anything else: true
            return wrapLog("default,true", true);
        }

    }
}
