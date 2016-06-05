using System;
using System.Collections.Generic;
using Hls.attribute_list;
using Txt.Core;

namespace Hls.EXT_X_STREAM_INF
{
    public class ExtStreamInfParser : Parser<ExtStreamInf, VariantStream>
    {
        private readonly IParser<AttributeList, IDictionary<string, object>> attributeListParser;

        public ExtStreamInfParser(IParser<AttributeList, IDictionary<string, object>> attributeListParser)
        {
            if (attributeListParser == null)
            {
                throw new ArgumentNullException(nameof(attributeListParser));
            }
            this.attributeListParser = attributeListParser;
        }

        protected override VariantStream ParseImpl(ExtStreamInf value)
        {
            var result = new VariantStream();
            var values = attributeListParser.Parse((AttributeList)value[1]);
            object tmp;
            if (!values.TryGetValue(@"BANDWIDTH", out tmp))
            {
                throw new InvalidOperationException("Every EXT-X-STREAM-INF tag MUST include the BANDWIDTH attribute.");
            }
            result.Bandwidth = (int)tmp;
            if (values.TryGetValue(@"AVERAGE-BANDWIDTH", out tmp))
            {
                result.AverageBandwidth = (int)tmp;
            }
            if (values.TryGetValue(@"CODECS", out tmp))
            {
                var codecs = (string)tmp;
                foreach (var codec in codecs.Split(','))
                {
                    result.Codecs.Add(codec);
                }
            }
            if (values.TryGetValue(@"RESOLUTION", out tmp))
            {
                result.Resolution = (Tuple<int, int>)tmp;
            }
            if (values.TryGetValue(@"FRAME-RATE", out tmp))
            {
                result.Framerate = (float)tmp;
            }
            if (values.TryGetValue(@"AUDIO", out tmp))
            {
                result.Audio = (string)tmp;
            }
            if (values.TryGetValue(@"VIDEO", out tmp))
            {
                result.Video = (string)tmp;
            }
            if (values.TryGetValue(@"SUBTITLES", out tmp))
            {
                result.Subtitles = (string)tmp;
            }
            if (values.TryGetValue(@"CLOSED-CAPTIONS", out tmp))
            {
                result.ClosedCaptions = (string)tmp;
            }
            result.Uri = new System.Uri(value[3].Text, UriKind.RelativeOrAbsolute);
            return result;
        }
    }
}
