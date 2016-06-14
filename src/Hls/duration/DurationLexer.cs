using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.duration
{
    public sealed class DurationLexer : CompositeLexer<Alternation, Duration>
    {
        public DurationLexer([NotNull] ILexer<Alternation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
