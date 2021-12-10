﻿namespace ToSic.Sxc.Web.PageFeatures
{
    public class BuiltInFeatures
    {
        /// <summary>
        /// JQuery feature
        /// </summary>
        /// <remarks>
        /// Note: officially published the key 'jQuery' in v12.02, do not change
        /// </remarks>
        public static PageFeature JQuery = new PageFeature("jQuery", "jQuery");
        
        /// <summary>
        /// Internal feature, not published ATM
        /// </summary>
        public static PageFeature PageContext = new PageFeature("2sxc.PageContext", "the $2sxc headers in the page so everything works");

        /// <summary>
        /// The core 2sxc JS libraries
        /// </summary>
        public static PageFeature Core = new PageFeature("2sxc.Core", "2sxc core js APIs", requires: new[]
        {
            PageContext.Key
        });

        /// <summary>
        /// WIP - this will probably be moved to local only in future, ATM it's global though
        /// </summary>
        public static PageFeature AutoToolbarGlobal = new PageFeature("2sxc.GlobalAutoToolbar", "Ensure that the toolbars automatically appear", requires: new[]
        {
            PageContext.Key
        });

        public static PageFeature EditApi = new PageFeature("2sxc.EditApi", "2sxc inpage editing APIs", requires: new[]
        {
            Core.Key
        });

        public static PageFeature EditUi =
            new PageFeature("2sxc.EditUi", "2sxc InPage editing UIs / Toolbar", requires: new[]
            {
                Core.Key,
                AutoToolbarGlobal.Key,
                EditApi.Key
            });

        // Note: officially published in 12.02
        public static PageFeature TurnOn = new PageFeature("turnOn", "turnOn JS library");

    }
}
