using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.title
{
    public sealed class TitleLexer : CompositeLexer<Repetition, Title>
    {
        public TitleLexer([NotNull] ILexer<Repetition> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
