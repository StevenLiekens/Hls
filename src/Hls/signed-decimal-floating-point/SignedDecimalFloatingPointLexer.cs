using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.signed_decimal_floating_point
{
    public sealed class SignedDecimalFloatingPointLexer : CompositeLexer<Concatenation, SignedDecimalFloatingPoint>
    {
        public SignedDecimalFloatingPointLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
