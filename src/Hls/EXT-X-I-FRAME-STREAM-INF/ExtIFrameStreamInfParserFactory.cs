using System;
using System.Collections.Generic;
using Hls.attribute_list;
using JetBrains.Annotations;
using Txt.Core;

namespace Hls.EXT_X_I_FRAME_STREAM_INF
{
    public class ExtIFrameStreamInfParserFactory : ParserFactory<ExtIFrameStreamInf, IntraFrameStreamInfo>
    {
        static ExtIFrameStreamInfParserFactory()
        {
            Default = new ExtIFrameStreamInfParserFactory(attribute_list.AttributeListParserFactory.Default.Singleton());
        }

        public ExtIFrameStreamInfParserFactory(
            [NotNull] IParserFactory<AttributeList, IDictionary<string, object>> attributeListParserFactory)
        {
            if (attributeListParserFactory == null)
            {
                throw new ArgumentNullException(nameof(attributeListParserFactory));
            }
            AttributeListParserFactory = attributeListParserFactory;
        }

        public static ExtIFrameStreamInfParserFactory Default { get; }

        public IParserFactory<AttributeList, IDictionary<string, object>> AttributeListParserFactory { get; }

        public override IParser<ExtIFrameStreamInf, IntraFrameStreamInfo> Create()
        {
            return new ExtIFrameStreamInfParser(AttributeListParserFactory.Create());
        }
    }
}
