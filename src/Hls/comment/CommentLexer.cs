using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.comment
{
    public sealed class CommentLexer : CompositeLexer<Concatenation, Comment>
    {
        public CommentLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
