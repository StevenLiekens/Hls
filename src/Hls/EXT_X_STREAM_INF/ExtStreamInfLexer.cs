using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXT_X_STREAM_INF
{
    public sealed class ExtStreamInfLexer : CompositeLexer<Concatenation, ExtStreamInf>
    {
        public ExtStreamInfLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
