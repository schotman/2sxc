﻿using System;
using System.Collections.Specialized;
using System.Linq;
using ToSic.Eav.Configuration;
using ToSic.Eav.Documentation;
using ToSic.Eav.Logging;
using ToSic.Razor.Blade;
using ToSic.Sxc.Data;
using ToSic.Sxc.Web.Url;
using static ToSic.Sxc.Images.ImageConstants;
using static ToSic.Sxc.Images.SrcSetPart;

namespace ToSic.Sxc.Images
{
    [PrivateApi("Internal stuff")]
    public class ImgResizeLinker : HasLog<ImgResizeLinker>
    {
        public ImgResizeLinker(Lazy<IFeaturesService> features) : base($"{Constants.SxcLogName}.ImgRes")
        {
            _features = features;
            DimGen = new ResizeDimensionGenerator().Init(Log);
        }
        private readonly Lazy<IFeaturesService> _features;

        public bool Debug = false;

        public readonly ResizeDimensionGenerator DimGen;

        /// <summary>
        /// Make sure this is in sync with the Link.Image
        /// </summary>
        public string Image(
            string url = default,
            object settings = default,
            object factor = default,
            string noParamOrder = Eav.Parameters.Protector,
            IDynamicField field = default,  // todo
            object width = default,
            object height = default,
            object quality = default,
            string resizeMode = default,
            string scaleMode = default,
            string format = default,
            object aspectRatio = default,
            string parameters = default
            )
        {
            var wrapLog = (Debug ? Log : null).SafeCall<string>($"{nameof(url)}:{url}");

            // Modern case - all settings have already been prepared, the other settings are ignored
            if (settings is ResizeSettings resizeSettings)
            {
                var basic = ImageOnly(url, resizeSettings, field).Url;
                return wrapLog("prepared:" + basic, basic);
            }

            resizeSettings = ResizeParamMerger.BuildResizeSettings(
                settings, factor, width: width, height: height, quality: quality, resizeMode: resizeMode,
                scaleMode: scaleMode, format: format, aspectRatio: aspectRatio,
                parameters: parameters, allowMulti: false);

            var result = ImageOnly(url, resizeSettings, field).Url;
            return wrapLog("built:" + result, result);
        }

        public OneResize ImageOnly(string url, ResizeSettings settings, IDynamicField field)
        {
            var wrapLog = Log.Call<OneResize>();
            var srcSetSettings = settings.Find(SrcSetType.Img, _features.Value.IsEnabled(FeaturesCatalog.ImageServiceUseFactors.NameId));
            return wrapLog("no srcset", ConstructUrl(url, settings, srcSetSettings, field: field));
        }


        public string SrcSet(string url, ResizeSettings settings, SrcSetType srcSetType, IDynamicField field = null)
        {
            var wrapLog = Log.Call<string>();

            var srcSetSettings = settings.Find(srcSetType, _features.Value.IsEnabled(FeaturesCatalog.ImageServiceUseFactors.NameId));

            var srcSetConfig = srcSetSettings?.SrcSetParsed;

            // Basic case -no srcSet config. In this case the src-set can just contain the url.
            if ((srcSetConfig?.Length ?? 0) == 0)
                return wrapLog("no srcset", ConstructUrl(url, settings, srcSetSettings, field: field).Url);

            var results = srcSetConfig.Select(ssConfig =>
            {
                if (ssConfig.SizeType == SizeDefault)
                    return ConstructUrl(url, settings, srcSetSettings, null);

                var oneResize = new OneResize
                {
                    Width = BestSrcSetDimension(settings.Width, ssConfig.Width, ssConfig,
                        FallbackWidthForSrcSet),
                    Height = BestSrcSetDimension(settings.Height, ssConfig.Height, ssConfig,
                    FallbackHeightForSrcSet)
                };

                var one = ConstructUrl(url, settings, srcSetSettings, field: field, preCalculated: oneResize);
                // this must happen at the end
                one.Suffix = SrcSetParser.SrcSetSuffix(ssConfig, one.Width);
                return one;
            });
            var result = string.Join(",\n", results.Select(r => r.UrlWithSuffix));

            return wrapLog("srcset", result);
        }


        /// <summary>
        /// Get the best matching dimension (width/height) based on what's specified
        /// </summary>
        private int BestSrcSetDimension(int original, int onSrcSet, SrcSetPart part, int fallbackIfNoOriginal)
        {
            // SrcSet defined a value, use that
            if (onSrcSet != 0) return onSrcSet;

            // No need to recalculate anything, return original
            if (part.SizeType != SizePixelDensity && part.SizeType != SizeFactorOf) return original;

            // If we're doing a factor-of, we always need an original value. If it's missing, use the fallback
            if (part.SizeType == SizeFactorOf && original == 0) original = fallbackIfNoOriginal;

            // Calculate the expected value based on Size=Scale-Factor * original
            return (int)(part.Size * original);
        }

        private OneResize ConstructUrl(string url, ResizeSettings resizeSettings, Recipe srcSetSettings, IDynamicField field, OneResize preCalculated = null)
        {
            var one = DimGen.ResizeDimensions(resizeSettings, srcSetSettings, preCalculated);
            one.TagEnhancements = srcSetSettings;

            var resizerNvc = new NameValueCollection();
            ImgAddIfRelevant(resizerNvc, "w", one.Width, "0");
            ImgAddIfRelevant(resizerNvc, "h", one.Height, "0");
            ImgAddIfRelevant(resizerNvc, "quality", resizeSettings.Quality, "0");
            ImgAddIfRelevant(resizerNvc, "mode", resizeSettings.ResizeMode, DontSetParam);
            ImgAddIfRelevant(resizerNvc, "scale", resizeSettings.ScaleMode, DontSetParam);
            ImgAddIfRelevant(resizerNvc, "format", resizeSettings.Format, DontSetParam);

            // Get resize instructions of the data if it has any
            var modifier = field?.ImageDecoratorOrNull()?.GetAnchorOrNull();
            if (modifier?.Item1 != null)
                ImgAddIfRelevant(resizerNvc, modifier.Value.Item1, modifier.Value.Item2);

            url = UrlHelpers.AddQueryString(url, resizerNvc);

            if (resizeSettings.Parameters != null && resizeSettings.Parameters.HasKeys())
                url = UrlHelpers.AddQueryString(url, resizeSettings.Parameters);

            var result = Tags.SafeUrl(url).ToString();
            one.Url = result;
            return one;
        }


        private bool ImgAddIfRelevant(NameValueCollection resizer, string key, object value, string irrelevant = "")
        {
            var wrapLog = (Debug ? Log : null).SafeCall<bool>();
            if (key == null || value == null)
                return wrapLog($"Won't add '{key}', since key or value are null", false);

            var strValue = value.ToString();
            if (string.IsNullOrEmpty(strValue))
                return wrapLog($"Won't add '{key}' since value as string would be null", false);

            if (strValue.Equals(irrelevant, StringComparison.InvariantCultureIgnoreCase))
                return wrapLog($"Won't add '{key}' since value would be irrelevant", false);

            resizer.Add(key, strValue);
            return wrapLog($"Added key {key}", true);
        }


        internal ResizeParamMerger ResizeParamMerger
        {
            get
            {
                if (_resizeParamMerger != null) return _resizeParamMerger;
                _resizeParamMerger = new ResizeParamMerger().Init(Log);
                if (Debug) _resizeParamMerger.Debug = true;
                return _resizeParamMerger;
            }
        }
        private ResizeParamMerger _resizeParamMerger;
        
    }
}
