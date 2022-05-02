﻿using System;
using DotNetNuke.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ToSic.Sxc.Dnn.StartUp
{
    /// <summary>
    /// This is the preferred way to start Dependency Injection, but it requires Dnn 9.4+
    /// If an older version of Dnn is used, this code will not run
    /// </summary>
    public class StartupDnn9 : IDnnStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Tell the static factory (used in obsolete code) about the services we have
            //Eav.Factory.UseExistingServices(services);

            // Do standard registration of all services
            // If Dnn < 9.4 is called, this will be called again from the Route-Registration code
            DnnDi.RegisterServices(services);

            // Give it the Dnn 9 Global Service Provider
            // https://github.com/dnnsoftware/Dnn.Platform/blob/9f83285a15d23203cbaad72d62add864ab5b8c7f/DNN%20Platform/DotNetNuke.Web/Common/LazyServiceProvider.cs#L28
            DnnDi.GetPreparedServiceProvider = () => typeof(DotNetNuke.Common.Globals).GetProperty("DependencyProvider", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)?.GetValue(null) as IServiceProvider;
        }
    }
}