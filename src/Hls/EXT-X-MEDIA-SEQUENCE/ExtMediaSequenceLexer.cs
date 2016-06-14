using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXT_X_MEDIA_SEQUENCE
{
    public sealed class ExtMediaSequenceLexer : CompositeLexer<Concatenation, ExtMediaSequence>
    {
        public ExtMediaSequenceLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
