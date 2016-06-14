using JetBrains.Annotations;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_DISCONTINUITY
{
    public sealed class ExtDiscontinuityLexer : CompositeLexer<Terminal, ExtDiscontinuity>
    {
        public ExtDiscontinuityLexer([NotNull] ILexer<Terminal> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
