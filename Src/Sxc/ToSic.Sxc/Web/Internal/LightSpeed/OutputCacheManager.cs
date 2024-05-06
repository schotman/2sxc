﻿using System.Runtime.Caching;
using ToSic.Eav.Apps.State;
using ToSic.Eav.Caching;
using ToSic.Eav.Internal.Features;
using ToSic.Lib.Services;

namespace ToSic.Sxc.Web.Internal.LightSpeed;

internal class OutputCacheManager(MemoryCacheService memoryCacheService, Lazy<IEavFeaturesService> featuresDoNotConnect) : ServiceBase(SxcLogName + ".OutputCacheManager", connect: [memoryCacheService])
{
    internal const string GlobalCacheRoot = "2sxc.Lightspeed.Module.";

    internal static string Id(int moduleId, int pageId, int? userId, string view, string suffix, string currentCulture)
    {
        var id = $"{GlobalCacheRoot}p:{pageId}-m:{moduleId}";
        if (userId.HasValue) id += $"-u:{userId.Value}";
        if (view != null) id += $"-v:{view}";
        if (suffix != null) id += $"-s:{suffix}";
        if (currentCulture != null) id += $"-c:{currentCulture}";
        return id;
    }

    public string Add(string cacheKey, OutputCacheItem data, int duration, List<IAppStateChanges> appStates, IList<string> appPaths, CacheEntryUpdateCallback updateCallback)
    {
        var l = Log.Fn<string>($"key: {cacheKey}", timer: true);
        try
        {
            // Never store 0, that's like never-expire
            if (duration == 0) duration = 1;
            var expiration = new TimeSpan(0, 0, duration);

            l.Do(message: $"cache set cacheKey:{cacheKey}", timer: true,
                action: () => memoryCacheService.Set(cacheKey, data,
                                    slidingExpiration: expiration,
                                    folderPaths: appPaths?.ToDictionary(p => p, p => true),
                                    appStates: appStates,
                                    featuresService: featuresDoNotConnect.Value,
                                    updateCallback: updateCallback)
            );

            return l.ReturnAsOk(cacheKey);
        }
        catch
        {
            /* ignore for now */
        }
        return l.ReturnAsError("error");
    }

    public OutputCacheItem Get(string key) => memoryCacheService.Get(key) as OutputCacheItem;
}