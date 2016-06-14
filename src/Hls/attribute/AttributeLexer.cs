using Txt.Core;
using Txt.ABNF;

namespace Hls.attribute
{
    public sealed class AttributeLexer : CompositeLexer<Concatenation, Attribute>
    {
        public AttributeLexer(ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
