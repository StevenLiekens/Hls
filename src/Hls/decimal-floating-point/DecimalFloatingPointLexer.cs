using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.decimal_floating_point
{
    public sealed class DecimalFloatingPointLexer : CompositeLexer<Concatenation, DecimalFloatingPoint>
    {
        public DecimalFloatingPointLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
