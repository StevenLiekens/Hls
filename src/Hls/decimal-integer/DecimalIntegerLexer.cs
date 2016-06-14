using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.decimal_integer
{
    public sealed class DecimalIntegerLexer : CompositeLexer<Repetition, DecimalInteger>
    {
        public DecimalIntegerLexer([NotNull] ILexer<Repetition> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
