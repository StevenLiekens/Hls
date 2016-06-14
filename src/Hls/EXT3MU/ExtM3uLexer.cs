using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXTM3U
{
    public sealed class ExtM3uLexer : CompositeLexer<Terminal, ExtM3u>
    {
        public ExtM3uLexer([NotNull] ILexer<Terminal> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
