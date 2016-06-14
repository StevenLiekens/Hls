using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.tag
{
    public sealed class TagLexer : CompositeLexer<Alternation, Tag>
    {
        public TagLexer([NotNull] ILexer<Alternation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
