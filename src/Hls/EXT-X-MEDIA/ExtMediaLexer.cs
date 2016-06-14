using JetBrains.Annotations;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_MEDIA
{
    public sealed class ExtMediaLexer : CompositeLexer<Concatenation, ExtMedia>
    {
        public ExtMediaLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
