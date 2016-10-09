using System;
using Hls.attribute_value;
using JetBrains.Annotations;
using Txt.Core;

namespace Hls.attribute
{
    public class AttributeParserFactory : ParserFactory<Attribute, Tuple<string, object>>
    {
        static AttributeParserFactory()
        {
            Default = new AttributeParserFactory(attribute_value.AttributeValueParserFactory.Default.Singleton());
        }

        public AttributeParserFactory([NotNull] IParserFactory<AttributeValue, object> attributeValueParserFactory)
        {
            if (attributeValueParserFactory == null)
            {
                throw new ArgumentNullException(nameof(attributeValueParserFactory));
            }
            AttributeValueParserFactory = attributeValueParserFactory;
        }

        public static IParserFactory<Attribute, Tuple<string, object>> Default { get; }

        public IParserFactory<AttributeValue, object> AttributeValueParserFactory { get; }

        public override IParser<Attribute, Tuple<string, object>> Create()
        {
            return new AttributeParser(AttributeValueParserFactory.Create());
        }
    }
}
