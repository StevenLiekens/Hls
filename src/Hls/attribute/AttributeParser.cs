using System;
using Hls.attribute_value;
using Txt.Core;

namespace Hls.attribute
{
    public class AttributeParser : Parser<Attribute, Tuple<string, object>>
    {
        private readonly IParser<AttributeValue, object> attributeValueParser;

        public AttributeParser(IParser<AttributeValue, object> attributeValueParser)
        {
            this.attributeValueParser = attributeValueParser;
        }

        protected override Tuple<string, object> ParseImpl(Attribute attribute)
        {
            var name = attribute[0].Text;
            var value = attributeValueParser.Parse((AttributeValue)attribute[2]);
            return new Tuple<string, object>(name, value);
        }
    }
}
