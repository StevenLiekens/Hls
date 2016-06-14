using JetBrains.Annotations;
using Txt.Core;
using Txt.ABNF;

namespace Hls.playlist
{
    public sealed class PlaylistLexer : CompositeLexer<Concatenation, Playlist>
    {
        public PlaylistLexer([NotNull] ILexer<Concatenation> innerLexer)
            : base(innerLexer)
        {
        }
    }
}
