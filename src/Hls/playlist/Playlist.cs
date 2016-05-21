using Txt.ABNF;

namespace Hls.playlist
{
    public class Playlist : Concatenation
    {
        public Playlist(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
