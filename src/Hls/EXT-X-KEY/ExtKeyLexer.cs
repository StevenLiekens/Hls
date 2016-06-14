using JetBrains.Annotations;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_KEY
{
    public sealed class ExtKeyLexer : CompositeLexer<Concatenation, ExtKey>
    {
        public ExtKeyLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
