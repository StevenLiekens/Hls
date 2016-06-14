using JetBrains.Annotations;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_ENDLIST
{
    public sealed class ExtEndListLexer : CompositeLexer<Terminal, ExtEndList>
    {
        public ExtEndListLexer([NotNull] ILexer<Terminal> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
