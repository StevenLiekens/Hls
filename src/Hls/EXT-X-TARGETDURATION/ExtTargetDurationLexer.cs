using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXT_X_TARGETDURATION
{
    public sealed class ExtTargetDurationLexer : CompositeLexer<Concatenation, ExtTargetDuration>
    {
        public ExtTargetDurationLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
