using Txt.ABNF;

namespace Hls.attribute_value
{
    public class AttributeValue : Alternation
    {
        public AttributeValue(Alternation Alternation)
            : base(Alternation)
        {
        }
    }
}
