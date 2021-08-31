﻿using Microsoft.AspNetCore.Http;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using System;
using System.Linq;

namespace ToSic.Sxc.Oqt.Server.Plumbing
{
    public class SiteStateInitializer
    {
        public Lazy<SiteState> SiteStateLazy { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public Lazy<IAliasRepository> AliasRepositoryLazy { get; }

        public SiteStateInitializer(Lazy<SiteState> siteStateLazy, IHttpContextAccessor httpContextAccessor,
            Lazy<IAliasRepository> aliasRepositoryLazy)
        {
            SiteStateLazy = siteStateLazy;
            HttpContextAccessor = httpContextAccessor;
            AliasRepositoryLazy = aliasRepositoryLazy;
        }
        
        /// <summary>
        /// Use this from inner code, which must always have an initialized state.
        /// Usually this has been ensured at the very top - when razor starts or when WebApi start
        /// </summary>
        public SiteState InitializedState 
        {
            get
            {
                if (SiteStateLazy.Value?.Alias != null) return SiteStateLazy.Value;
                InitIfEmpty();
                return SiteStateLazy.Value;
            }
        }

        /// <summary>
        /// Will initialize the SiteState if it has not been initialized yet
        /// </summary>
        /// <returns></returns>
        internal bool InitIfEmpty(int? siteId = null)
        {
            var siteState = SiteStateLazy.Value;

            // This would indicate it was called improperly, because we need the shared SiteState variable to work properly
            if (siteState == null) throw new ArgumentNullException(nameof(siteState));

            // Check if alias already set, in which case we skip this
            if (siteState.Alias != null) return true;

            // For anything else we need the httpContext, otherwise skip
            var request = HttpContextAccessor?.HttpContext?.Request;
            if (request == null) return false;

            // Try HACK
            object alias = null;
            if ((HttpContextAccessor?.HttpContext?.Items.TryGetValue("AliasFor2sxc", out alias) ?? false) && alias != null)
            {
                siteState.Alias = (Alias) alias;
                return false;
            }

            // Try get alias with info for HttpRequest and eventual SiteId.
            if (request.Path.HasValue && request.Path.Value != null && request.Path.Value.Contains("/_blazor"))
            {
                var url = $"{request.Host}";

                var aliases = AliasRepositoryLazy.Value.GetAliases().ToList(); // cached by Oqtane

                if (siteId.HasValue) // acceptable solution
                    siteState.Alias = aliases.OrderByDescending(a => a.Name.Length)
                        .ThenBy(a => a.Name)
                        .FirstOrDefault(a => a.SiteId == siteId.Value && a.Name.StartsWith(url, StringComparison.InvariantCultureIgnoreCase));
                else // fallback solution, wrong site is possible
                    siteState.Alias = aliases.OrderByDescending(a => a.Name)
                        .FirstOrDefault(a => a.Name.StartsWith(url, StringComparison.InvariantCultureIgnoreCase));
            }
            else // great solution
            {
                var url = $"{request.Host}{request.Path}";

                var aliases = AliasRepositoryLazy.Value.GetAliases().ToList(); // cached by Oqtane
                siteState.Alias = aliases.OrderByDescending(a => a.Name.Length)
                    .ThenBy(a => a.Name)
                    .FirstOrDefault(a => url.StartsWith(a.Name, StringComparison.InvariantCultureIgnoreCase));
            }

            return siteState.Alias != null;
        }

    }
}
