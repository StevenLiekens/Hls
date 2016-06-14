using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXTINF
{
    public sealed class ExtInfLexer : CompositeLexer<Concatenation, ExtInf>
    {
        public ExtInfLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
