using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Txt.Core;
using Attribute = Hls.attribute.Attribute;

namespace Hls.attribute_list
{
    public class AttributeListParserFactory : ParserFactory<AttributeList, IDictionary<string, object>>
    {
        static AttributeListParserFactory()
        {
            Default = new AttributeListParserFactory(attribute.AttributeParserFactory.Default.Singleton());
        }

        public AttributeListParserFactory(
            [NotNull] IParserFactory<Attribute, Tuple<string, object>> attributeParserFactory)
        {
            if (attributeParserFactory == null)
            {
                throw new ArgumentNullException(nameof(attributeParserFactory));
            }
            AttributeParserFactory = attributeParserFactory;
        }

        public static IParserFactory<AttributeList, IDictionary<string, object>> Default { get; }

        public IParserFactory<Attribute, Tuple<string, object>> AttributeParserFactory { get; }

        public override IParser<AttributeList, IDictionary<string, object>> Create()
        {
            return new AttributeListParser(AttributeParserFactory.Create());
        }
    }
}
