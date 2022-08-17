﻿using System;
using ToSic.Eav.Plumbing;

namespace ToSic.Sxc.Edit.Toolbar
{
    public partial class ToolbarBuilder
    {
        private IToolbarBuilder With(
            string noParamOrder = Eav.Parameters.Protector,
            string mode = null,
            object target = null,
            bool? condition = null, 
            Func<bool> conditionFunc = null,
            bool? force = null,
            string group = null
        )
        {
            Eav.Parameters.Protect(noParamOrder, $"{nameof(mode)}, {nameof(target)}, {nameof(condition)}, {nameof(conditionFunc)}");
            // Create clone before starting to log so it's in there too
            var clone = target == null 
                ? new ToolbarBuilder(this)
                : (ToolbarBuilder)Parameters(target);   // Params will already copy/clone it

            var p = clone._configuration = new ToolbarBuilderConfiguration(
                _configuration,
                mode: mode, 
                condition: condition, 
                conditionFunc: conditionFunc, 
                force: force, 
                group: group
            );
            return clone;
        }

        public IToolbarBuilder More(
            string noParamOrder = Eav.Parameters.Protector,
            object ui = null
        )
        {
            Eav.Parameters.Protect(noParamOrder, nameof(ui));
            return AddInternal(new ToolbarRuleCustom("more", ui: PrepareUi(ui)));
        }

        public IToolbarBuilder For(object target) => With(target: target);

        public IToolbarBuilder Target(object target) => With(target: target);

        public IToolbarBuilder Condition(bool condition) => With(condition: condition);

        public IToolbarBuilder Condition(Func<bool> condition) => With(conditionFunc: condition);

        public IToolbarBuilder Group(string name = null)
        {
            // Auto-determine the group name if none was provided
            // Maybe? only on null, because "" would mean to reset it again?
            if (!name.HasValue())
                name = _configuration?.Group.HasValue() == true
                    ? _configuration?.Group + "*" // add a uncommon character so each group has another name
                    : "custom";

            // Note that we'll add the new buttons directly using AddInternal so it won't
            // auto-add other UI params such as the previous group
            return name.StartsWith("-")
                // It's a remove-group rule
                ? AddInternal($"-group={name.Substring(1)}") 
                // It's an add group - set the current group and add the button-rule
                : With(group: name).AddInternal($"+group={name}");
        }
    }
}
