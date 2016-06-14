using JetBrains.Annotations;
using Txt.ABNF;
using Txt.Core;

namespace Hls.line
{
    public sealed class LineLexer : CompositeLexer<Concatenation, Line>
    {
        public LineLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
