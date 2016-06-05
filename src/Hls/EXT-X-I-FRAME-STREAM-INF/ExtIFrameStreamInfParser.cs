using System;
using System.Collections.Generic;
using Hls.attribute_list;
using Txt.Core;

namespace Hls.EXT_X_I_FRAME_STREAM_INF
{
    public class ExtIFrameStreamInfParser : Parser<ExtIFrameStreamInf, IntraFrameStreamInfo>
    {
        private readonly IParser<AttributeList, IDictionary<string, object>> attributeListParser;

        public ExtIFrameStreamInfParser(IParser<AttributeList, IDictionary<string, object>> attributeListParser)
        {
            if (attributeListParser == null)
            {
                throw new ArgumentNullException(nameof(attributeListParser));
            }
            this.attributeListParser = attributeListParser;
        }

        protected override IntraFrameStreamInfo ParseImpl(ExtIFrameStreamInf value)
        {
            var result = new IntraFrameStreamInfo();
            var values = attributeListParser.Parse((AttributeList)value[1]);
            object tmp;
            if (!values.TryGetValue(@"BANDWIDTH", out tmp))
            {
                throw new InvalidOperationException(
                    "Every EXT-X-I-FRAME-STREAM-INF tag MUST include the BANDWIDTH attribute.");
            }
            result.Bandwidth = (int)tmp;
            if (!values.TryGetValue(@"URI", out tmp))
            {
                throw new InvalidOperationException(
                    "Every EXT-X-I-FRAME-STREAM-INF tag MUST include the URI attribute.");
            }
            result.Uri = new System.Uri((string)tmp, UriKind.RelativeOrAbsolute);
            if (values.TryGetValue(@"AVERAGE-BANDWIDTH", out tmp))
            {
                result.AverageBandwidth = (int)tmp;
            }
            if (values.TryGetValue(@"CODECS", out tmp))
            {
                var codecs = (string)tmp;
                foreach (var codec in codecs.Split(','))
                {
                    result.Codecs.Add(codec.Trim());
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
            if (values.TryGetValue(@"VIDEO", out tmp))
            {
                result.Video = (string)tmp;
            }
            return result;
        }
    }
}
