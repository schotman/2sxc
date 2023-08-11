﻿using System;
using System.Collections.Generic;
using ToSic.Lib.Documentation;
using ToSic.Razor.Markup;
using static ToSic.Eav.Parameters;

// ReSharper disable ConvertToNullCoalescingCompoundAssignment

namespace ToSic.Sxc.Data
{
    public abstract partial class DynamicEntityBase: ITyped
    {
        [PrivateApi]
        internal CodeItemHelper ItemHelper => _itemHelper ?? (_itemHelper = new CodeItemHelper(Helper));
        private CodeItemHelper _itemHelper;

        [PrivateApi]
        bool ITyped.ContainsKey(string name)
        {
            return false; // must be overriden by implementation
        }

        public IEnumerable<string> Keys(string noParamOrder = Protector, IEnumerable<string> only = default)
        {
            throw new NotImplementedException();
        }

        [PrivateApi]
        object ITyped.Get(string name, string noParamOrder, bool? required) 
            => ItemHelper.Get(name, noParamOrder, required);

        [PrivateApi]
        TValue ITyped.Get<TValue>(string name, string noParamOrder, TValue fallback, bool? required)
            => ItemHelper.G4T(name, noParamOrder, fallback: fallback, required: required);

        [PrivateApi]
        IRawHtmlString ITyped.Attribute(string name, string noParamOrder, string fallback, bool? required) 
            => ItemHelper.Attribute(name, noParamOrder, fallback, required);

        [PrivateApi]
        dynamic ITyped.Dyn => this;


        [PrivateApi]
        DateTime ITyped.DateTime(string name, string noParamOrder, DateTime fallback, bool? required)
            => ItemHelper.G4T(name, noParamOrder: noParamOrder, fallback: fallback, required: required);

        [PrivateApi]
        string ITyped.String(string name, string noParamOrder, string fallback, bool? required, bool scrubHtml) 
            => ItemHelper.String(name, noParamOrder, fallback, required, scrubHtml);

        [PrivateApi]
        int ITyped.Int(string name, string noParamOrder, int fallback, bool? required)
            => ItemHelper.G4T(name, noParamOrder: noParamOrder, fallback: fallback, required: required);

        [PrivateApi]
        bool ITyped.Bool(string name, string noParamOrder, bool fallback, bool? required) 
            => ItemHelper.G4T(name, noParamOrder: noParamOrder, fallback: fallback, required: required);

        [PrivateApi]
        long ITyped.Long(string name, string noParamOrder, long fallback, bool? required)
            => ItemHelper.G4T(name, noParamOrder: noParamOrder, fallback: fallback, required: required);

        [PrivateApi]
        float ITyped.Float(string name, string noParamOrder, float fallback, bool? required)
            => ItemHelper.G4T(name, noParamOrder: noParamOrder, fallback: fallback, required: required);

        [PrivateApi]
        decimal ITyped.Decimal(string name, string noParamOrder, decimal fallback, bool? required)
            => ItemHelper.G4T(name, noParamOrder: noParamOrder, fallback: fallback, required: required);

        [PrivateApi]
        double ITyped.Double(string name, string noParamOrder, double fallback, bool? required)
            => ItemHelper.G4T(name, noParamOrder: noParamOrder, fallback: fallback, required: required);

        [PrivateApi]
        string ITyped.Url(string name, string noParamOrder, string fallback, bool? required) 
            => ItemHelper.Url(name, noParamOrder, fallback, required);

        [PrivateApi]
        string ITyped.ToString() => "test / debug: " + ToString();
    }
}
