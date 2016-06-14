using JetBrains.Annotations;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_I_FRAME_STREAM_INF
{
    public sealed class ExtIFrameStreamInfLexer : CompositeLexer<Concatenation, ExtIFrameStreamInf>
    {
        public ExtIFrameStreamInfLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
