﻿using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Oqtane.Repository;
using Oqtane.Shared;
using System;
using System.Linq;
using ToSic.Eav.Logging;
using ToSic.Eav.Plumbing;
using ToSic.Sxc.Oqt.Shared.Dev;
using Log = ToSic.Eav.Logging.Simple.Log;

namespace ToSic.Sxc.Oqt.Server.Controllers
{
    /// <summary>
    /// Api controllers normally should inherit ControllerBase but we have a special case of inhering from Controller.
    /// It is because our custom dynamic 2sxc app api controllers (without constructor), depends on event OnActionExecuting
    /// to provide dependencies (without DI in constructor).
    /// </summary>
    public abstract class OqtControllerBase : Controller, IHasLog
    {
        protected OqtControllerBase()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            // todo: redesign so it works - in .net core the HttpContext isn't ready in the constructor
            Log = new Log(HistoryLogName, null, $"Path: {HttpContext?.Request.GetDisplayUrl()}");
            // ReSharper disable once VirtualMemberCallInConstructor
            History.Add(HistoryLogGroup, Log);
            // todo: get this to work
        }

        /// <inheritdoc />
        public ILog Log { get; }

        /// <summary>
        /// The group name for log entries in insights.
        /// Helps group various calls by use case.
        /// </summary>
        protected virtual string HistoryLogGroup { get; } = "web-api";

        /// <summary>
        /// The name of the logger in insights.
        /// The inheriting class should provide the real name to be used.
        /// </summary>
        protected abstract string HistoryLogName { get; }

        protected SiteState SiteState { get; private set; }


        [NonAction]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var serviceProvider = context.HttpContext.RequestServices;
            SiteState = serviceProvider.Build<SiteState>();

            // background processes can pass in an alias using the SiteState service
            if (SiteState.Alias != null) return;

            var request = context.HttpContext.Request;
            var host = $"{request.Host}";
            var url = $"{request.Host}{request.Path}";

            var siteId = -1;

            // get aliasId identifier based on request
            var segments = request.Path.Value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments != null && segments.Length > 1 && (segments[1] == "api" || segments[1] == "pages") && segments[0] != "~")
            {
                siteId = int.Parse(segments[0]);
            }

            // get the alias
            var aliasRepository = serviceProvider.Build<IAliasRepository>();
            var aliases = aliasRepository.GetAliases().ToList(); // cached
            if (siteId != -1)
                SiteState.Alias = aliases.OrderBy(a => a.Name)
                    .FirstOrDefault(a => a.SiteId == siteId && a.Name.StartsWith(host));
            else
                SiteState.Alias = aliases.OrderByDescending(a => a.Name.Length)
                    .ThenBy(a => a.Name)
                    .FirstOrDefault(a => url.StartsWith(a.Name));
        }

        #region Extend Time so Web Server doesn't time out - not really implemented ATM

        protected void PreventServerTimeout300() => WipConstants.DontDoAnythingImplementLater();

        #endregion
    }

}
