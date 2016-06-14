using JetBrains.Annotations;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_DISCONTINUITY_SEQUENCE
{
    public sealed class ExtDiscontinuitySequenceLexer : CompositeLexer<Concatenation, ExtDiscontinuitySequence>
    {
        public ExtDiscontinuitySequenceLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
