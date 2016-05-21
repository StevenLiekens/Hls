using Txt.ABNF;

namespace Hls.attribute_list
{
    public class AttributeList : Concatenation
    {
        public AttributeList(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
