using System;
using System.Collections.Generic;
using Hls.attribute_list;
using JetBrains.Annotations;
using Txt.Core;

namespace Hls.EXT_X_STREAM_INF
{
    public class ExtStreamInfParserFactory : ParserFactory<ExtStreamInf, VariantStream>
    {
        static ExtStreamInfParserFactory()
        {
            Default = new ExtStreamInfParserFactory(attribute_list.AttributeListParserFactory.Default.Singleton());
        }

        public ExtStreamInfParserFactory(
            [NotNull] IParserFactory<AttributeList, IDictionary<string, object>> attributeListParserFactory)
        {
            if (attributeListParserFactory == null)
            {
                throw new ArgumentNullException(nameof(attributeListParserFactory));
            }
            AttributeListParserFactory = attributeListParserFactory;
        }

        public static ExtStreamInfParserFactory Default { get; }

        public IParserFactory<AttributeList, IDictionary<string, object>> AttributeListParserFactory { get; }

        public override IParser<ExtStreamInf, VariantStream> Create()
        {
            return new ExtStreamInfParser(AttributeListParserFactory.Create());
        }
    }
}
