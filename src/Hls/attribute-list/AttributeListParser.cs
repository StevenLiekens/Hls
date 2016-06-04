using System;
using System.Collections.Generic;
using Txt.Core;
using Attribute = Hls.attribute.Attribute;

namespace Hls.attribute_list
{
    public class AttributeListParser : Parser<AttributeList, IDictionary<string, object>>
    {
        private readonly IParser<Attribute, Tuple<string, object>> attributeParser;

        public AttributeListParser(IParser<Attribute, Tuple<string, object>> attributeParser)
        {
            if (attributeParser == null)
            {
                throw new ArgumentNullException(nameof(attributeParser));
            }
            this.attributeParser = attributeParser;
        }

        protected override IDictionary<string, object> ParseImpl(AttributeList attributeList)
        {
            var result = new Dictionary<string, object>();
            var first = attributeParser.Parse((Attribute)attributeList[0]);
            result.Add(first.Item1, first.Item2);
            foreach (var concatenation in attributeList[1])
            {
                var next = attributeParser.Parse((Attribute)concatenation[1]);
                result.Add(next.Item1, next.Item2);
            }
            return result;
        }
    }
}
