﻿using System.Text.Json;

namespace ToSic.Sxc.WebApi
{
    public class JsonFormatterHelpers
    {
        public static void SetCasing(Casing casing, JsonSerializerOptions jsonSerializerOptions)
        {
            var objectPreserve = casing.HasFlag(Casing.Preserve);
            jsonSerializerOptions.PropertyNamingPolicy = objectPreserve ? null : JsonNamingPolicy.CamelCase;

            var dicPreserve = objectPreserve || casing.HasFlag(Casing.DictionaryPreserve);
            jsonSerializerOptions.DictionaryKeyPolicy = dicPreserve ? null : JsonNamingPolicy.CamelCase;
        }
    }
}
