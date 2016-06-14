using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.hexadecimal_sequence
{
    public sealed class HexadecimalSequenceLexer : CompositeLexer<Concatenation, HexadecimalSequence>
    {
        public HexadecimalSequenceLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
