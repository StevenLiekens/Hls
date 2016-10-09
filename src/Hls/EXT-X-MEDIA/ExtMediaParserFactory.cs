using System;
using System.Collections.Generic;
using Hls.attribute_list;
using JetBrains.Annotations;
using Txt.Core;

namespace Hls.EXT_X_MEDIA
{
    public class ExtMediaParserFactory : ParserFactory<ExtMedia, Rendition>
    {
        static ExtMediaParserFactory()
        {
            Default = new ExtMediaParserFactory(attribute_list.AttributeListParserFactory.Default.Singleton());
        }

        public ExtMediaParserFactory(
            [NotNull] IParserFactory<AttributeList, IDictionary<string, object>> attributeListParserFactory)
        {
            if (attributeListParserFactory == null)
            {
                throw new ArgumentNullException(nameof(attributeListParserFactory));
            }
            AttributeListParserFactory = attributeListParserFactory;
        }

        public static ExtMediaParserFactory Default { get; }

        public IParserFactory<AttributeList, IDictionary<string, object>> AttributeListParserFactory { get; }

        public override IParser<ExtMedia, Rendition> Create()
        {
            return new ExtMediaParser(AttributeListParserFactory.Create());
        }
    }
}
