using JetBrains.Annotations;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EOL
{
    public sealed class EndOfLineLexer : CompositeLexer<Alternation, EndOfLine>
    {
        public EndOfLineLexer([NotNull] ILexer<Alternation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
