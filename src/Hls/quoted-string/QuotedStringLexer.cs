using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.quoted_string
{
    public sealed class QuotedStringLexer : CompositeLexer<Concatenation, QuotedString>
    {
        public QuotedStringLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
