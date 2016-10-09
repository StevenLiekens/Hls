using System;
using System.Collections.Generic;
using Hls.attribute_list;
using JetBrains.Annotations;
using Txt.Core;

namespace Hls.EXT_X_KEY
{
    public class ExtKeyParserFactory : ParserFactory<ExtKey, Key>
    {
        static ExtKeyParserFactory()
        {
            Default = new ExtKeyParserFactory(attribute_list.AttributeListParserFactory.Default.Singleton());
        }

        public ExtKeyParserFactory(
            [NotNull] IParserFactory<AttributeList, IDictionary<string, object>> attributeListParserFactory)
        {
            if (attributeListParserFactory == null)
            {
                throw new ArgumentNullException(nameof(attributeListParserFactory));
            }
            AttributeListParserFactory = attributeListParserFactory;
        }

        public static ExtKeyParserFactory Default { get; }

        public IParserFactory<AttributeList, IDictionary<string, object>> AttributeListParserFactory { get; }

        public override IParser<ExtKey, Key> Create()
        {
            return new ExtKeyParser(AttributeListParserFactory.Create());
        }
    }
}
