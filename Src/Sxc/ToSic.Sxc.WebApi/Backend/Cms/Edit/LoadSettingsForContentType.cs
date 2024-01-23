﻿using ToSic.Eav.Plumbing;
using ToSic.Sxc.Internal;
using static System.String;

namespace ToSic.Sxc.Backend.Cms;

internal class LoadSettingsForContentType()
    : LoadSettingsProviderBase($"{SxcLogging.SxcLogName}.LdStCT"), ILoadSettingsProvider
{
    public Dictionary<string, object> GetSettings(LoadSettingsProviderParameters parameters) => Log.Func(l =>
    {
        // find all keys which may be necessary
        var settingsKeys = parameters.ContentTypes
            .SelectMany(ct => (ct.Metadata.DetailsOrNull?.AdditionalSettings ?? "")
                .CsvToArrayWithoutEmpty()
                //.Split(',')
                //.Select(s => s.Trim())
            )
            .Where(c => !IsNullOrWhiteSpace(c))
            // Only include settings which have the full path
            // so in future we can add other roots like resources
            .Where(s => s.StartsWith($"{AppStackConstants.RootNameSettings}."))
            .ToList();

        // Try to find each setting
        var settings = SettingsByKeys(parameters.ContextOfApp.AppSettings, settingsKeys);

        return (settings, $"{settings.Count}");
    });
}