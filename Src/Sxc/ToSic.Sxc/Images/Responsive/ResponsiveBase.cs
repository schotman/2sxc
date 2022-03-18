﻿using System.Linq;
using ToSic.Eav;
using ToSic.Razor.Blade;
using ToSic.Razor.Html5;
using ToSic.Sxc.Plumbing;
using ToSic.Sxc.Web;
// ReSharper disable ConvertToNullCoalescingCompoundAssignment

namespace ToSic.Sxc.Images
{
    public abstract class ResponsiveBase: HybridHtmlString
    {
        protected ResponsiveBase(
            ImageService imgService, 
            string url, 
            object settings, 
            // ReSharper disable once UnusedParameter.Local
            string noParamOrder = Parameters.Protector, 
            object factor = null, 
            RecipeSet mrs = null,
            string imgAlt = null,
            string imgClass = null,
            string logName = Constants.SxcLogName + ".IPSBas"
            )
        {
            ImgService = imgService;
            FactorParam = factor;
            ImgAlt = imgAlt;
            ImgClass = imgClass;
            ImgLinker = imgService.ImgLinker;
            UrlOriginal = url;
            Settings = PrepareResizeSettings(settings, factor, mrs);

        }
        protected readonly ImgResizeLinker ImgLinker;
        protected readonly ImageService ImgService;
        protected readonly object FactorParam;
        protected readonly string ImgAlt;
        protected readonly string ImgClass;
        protected readonly string UrlOriginal;

        public string Url => ThisResize.Url;

        protected OneResize ThisResize => _thisResize ?? (_thisResize = ImgLinker.ImageOnly(UrlOriginal, Settings, SrcSetType.Img));
        private OneResize _thisResize;

        internal ResizeSettings Settings { get; }


        protected ResizeSettings PrepareResizeSettings(object settings, object factor, RecipeSet mrs)
        {
            // 1. Prepare Settings
            if (settings is ResizeSettings resSettings)
            {
                // If we have a modified factor, make sure we have that (this will copy the settings)
                var newFactor = ParseObject.DoubleOrNullWithCalculation(factor);
                resSettings = new ResizeSettings(resSettings, factor: newFactor ?? resSettings.Factor, mrs);
            }
            else
                resSettings = ImgLinker.ResizeParamMerger.BuildResizeSettings(settings, factor: factor, allowMulti: true, advanced: mrs);

            return resSettings;
        }

        /// <summary>
        /// ToString must be specified by each implementation
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        public virtual Img Img
        {
            get
            {
                if (_imgTag != null) return _imgTag;

                _imgTag = Tag.Img().Src(Url);

                // Add all kind of attributes if specified
                var tag = ThisResize.TagEnhancements;
                var dic = tag?.Attributes?
                    .Where(pair => !Recipe.SpecialProperties.Contains(pair.Key))
                    .ToDictionary(p => p.Key, p => p.Value); ;
                if (dic != null)
                    foreach (var a in dic)
                        _imgTag.Attr(a.Key, a.Value);

                // Only add these if they were really specified
                if (ImgAlt != null) _imgTag.Alt(ImgAlt);
                
                // Note that adding a class will keep previous class added
                if (ImgClass != null) _imgTag.Class(ImgClass);

                // Optionally set width and height if known
                if (tag?.SetWidth == true && ThisResize.Width != 0) _imgTag.Width(ThisResize.Width);
                if (tag?.SetHeight == true && ThisResize.Height != 0) _imgTag.Height(ThisResize.Height);

                return _imgTag;
            }
        }

        private Img _imgTag;

    }
}
