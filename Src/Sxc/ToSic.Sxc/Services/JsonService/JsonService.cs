﻿using ToSic.Eav.Serialization;
using System.Text.Json;
using ToSic.Eav;
using ToSic.Lib.Documentation;
using ToSic.Lib.Services;
using ToSic.Sxc.Data;

namespace ToSic.Sxc.Services
{
    [PrivateApi("Hide implementation")]
    internal class JsonService: ServiceBase, IJsonService
    {
        public JsonService(): base("Sxc.JsnSvc")
        {

        }

        /// <inheritdoc />
        public T To<T>(string json) 
            => JsonSerializer.Deserialize<T>(json, JsonOptions.SafeJsonForHtmlAttributes);

        /// <inheritdoc />
        public object ToObject(string json)
            => JsonSerializer.Deserialize<object>(json, JsonOptions.SafeJsonForHtmlAttributes);

        /// <inheritdoc />
        public ITypedRead ToTyped(string json, string noParamOrder = Parameters.Protector, string fallback = default) 
            => DynamicJacket.AsDynamicJacket(json, fallback, Log);

        /// <inheritdoc />
        public string ToJson(object item)
            => JsonSerializer.Serialize(item, JsonOptions.SafeJsonForHtmlAttributes);

        /// <inheritdoc />
        public string ToJson(object item, int indentation)
            => JsonSerializer.Serialize(item, JsonOptions.SafeJsonForHtmlAttributes);
    }
}
