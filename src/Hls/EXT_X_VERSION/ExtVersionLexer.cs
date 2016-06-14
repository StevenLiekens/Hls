using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXT_X_VERSION
{
    public sealed class ExtVersionLexer : CompositeLexer<Concatenation, ExtVersion>
    {
        public ExtVersionLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
