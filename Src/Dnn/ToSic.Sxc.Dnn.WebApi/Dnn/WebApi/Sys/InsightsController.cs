﻿using System.Web.Http;
using ToSic.Sxc.Dnn.WebApi.Logging;

namespace ToSic.Sxc.Dnn.WebApi.Sys
{
    [DnnLogExceptions]
    public class InsightsController : DnnApiControllerWithFixes
    {
        #region Logging
        protected override string HistoryLogName => "Api.Debug";

        /// <summary>
        /// Enable/disable logging of access to insights
        /// Only enable this if you have trouble developing insights, otherwise it clutters our logs
        /// </summary>
        internal const bool InsightsLoggingEnabled = false;

        internal const string InsightsUrlFragment = "/sys/insights/";

        /// <summary>
        /// Make sure that these requests don't land in the normal api-log.
        /// Otherwise each log-access would re-number what item we're looking at
        /// </summary>
        protected override string HistoryLogGroup => "web-api.insights";

        #endregion

        /// <summary>
        /// Single-Point-Of-Entry
        /// The insights handle all their work in the backend, incl. view-switching.
        /// This is important for many reasons, inc. the fact that this will always be the first endpoint to implement
        /// on any additional system. 
        /// </summary>
        [HttpGet]
        public string Details(string view, int? appId = null, string key = null, int? position = null, string type = null, bool? toggle = null, string nameId = null)
            => GetService<Sxc.Web.WebApi.System.Insights>().Init(Log)
                .Details(view, appId, key, position, type, toggle, nameId);

        
    }
}