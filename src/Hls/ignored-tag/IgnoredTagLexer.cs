using JetBrains.Annotations;
using Txt.ABNF;
using Txt.Core;

namespace Hls.ignored_tag
{
    public sealed class IgnoredTagLexer : CompositeLexer<Concatenation, IgnoredTag>
    {
        public IgnoredTagLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
