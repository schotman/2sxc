﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ToSic.Sxc.Dnn.Razor;
using ToSic.Sxc.Engines;

namespace ToSic.Sxc;

public static class StartUpDnnRazor
{
    public static IServiceCollection AddDnnRazor(this IServiceCollection services)
    {
        // Settings / WebApi stuff
        services.TryAddTransient<DnnRazorSourceAnalyzer>();
        services.TryAddTransient<HtmlHelper>();

        return services;
    }

}