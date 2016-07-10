using System;

namespace Hls
{
    public class PlaylistContext
    {
        public Uri Location { get; private set; }

        public PlaylistFile Playlist { get; set; }
    }
}
