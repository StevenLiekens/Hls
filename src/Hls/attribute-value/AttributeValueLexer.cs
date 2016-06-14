using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.attribute_value
{
    public sealed class AttributeValueLexer : CompositeLexer<Alternation, AttributeValue>
    {
        public AttributeValueLexer([NotNull] ILexer<Alternation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
