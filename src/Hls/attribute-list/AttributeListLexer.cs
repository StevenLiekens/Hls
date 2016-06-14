using Txt.Core;
using Txt.ABNF;

namespace Hls.attribute_list
{
    public sealed class AttributeListLexer : CompositeLexer<Concatenation, AttributeList>
    {
        public AttributeListLexer(ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
