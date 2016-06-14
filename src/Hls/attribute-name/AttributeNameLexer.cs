using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.attribute_name
{
    public sealed class AttributeNameLexer : CompositeLexer<Repetition, AttributeName>
    {
        public AttributeNameLexer([NotNull] ILexer<Repetition> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
