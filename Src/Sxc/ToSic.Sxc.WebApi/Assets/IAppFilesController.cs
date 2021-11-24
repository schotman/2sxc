﻿using System.Collections.Generic;
using ToSic.Sxc.Apps.Assets;

namespace ToSic.Sxc.WebApi.Assets
{
    public interface IAppFilesController
    {
        List<string> All(int appId, bool global, string path = null, string mask = "*.*", bool withSubfolders = false, bool returnFolders = false);

        /// <summary>
        /// Get details and source code
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="global">this determines, if the app-file store is the global in _default or the local in the current app</param>
        /// <param name="path"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        AssetEditInfo Asset(int appId, 
            int templateId = 0, string path = null, // identifier is always one of these two
            bool global = false);

        /// <summary>
        /// Update an asset with POST
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templateId"></param>
        /// <param name="global">this determines, if the app-file store is the global in _default or the local in the current app</param>
        /// <param name="path"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        bool Asset(int appId, AssetEditInfo template,
            int templateId = 0, string path = null, // identifier is either template Id or path
            // todo w/SPM - global never seems to be used - must check why and if we remove or add to UI
            // TODO: NEW PARAM TEMPLATEKey SHOULD BE USED TO CREATE THE FILE
            bool global = false);

        /// <summary>
        /// Create a new file (if it doesn't exist yet) and optionally prefill it with content
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="global">this determines, if the app-file store is the global in _default or the local in the current app</param>
        /// <param name="purpose">auto;razor;token;api;search</param>
        /// <returns></returns>
        bool Create(
            int appId,
            string path,
            FileContentsDto content, // note: as of 2020-09 the content is never submitted
            bool global,
            string purpose = Purpose.Auto);

        /// <summary>
        /// Get all asset template types
        /// </summary>
        /// <param name="purpose">filter by Purpose when provided</param>
        /// <returns></returns>
        TemplatesDto GetTemplates(string purpose = null);

        TemplatePreviewDto Preview(int appId, string path, string name, string templateKey, bool global = false);

        /// <summary>
        /// Create a new file from template
        /// </summary>
        /// <param name="assetFromTemplateDto">AssetFromTemplateDto</param>
        /// <returns></returns>
        bool CreateTemplate(AssetFromTemplateDto assetFromTemplateDto);
    }
}