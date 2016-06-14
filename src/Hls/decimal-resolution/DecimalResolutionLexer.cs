using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.decimal_resolution
{
    public sealed class DecimalResolutionLexer : CompositeLexer<Concatenation, DecimalResolution>
    {
        public DecimalResolutionLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
