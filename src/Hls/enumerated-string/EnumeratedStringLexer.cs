using JetBrains.Annotations;
using Txt.ABNF;
using Txt.Core;

namespace Hls.enumerated_string
{
    public sealed class EnumeratedStringLexer : CompositeLexer<Repetition, EnumeratedString>
    {
        public EnumeratedStringLexer([NotNull] ILexer<Repetition> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
