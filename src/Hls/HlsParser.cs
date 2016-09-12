using System;
using Hls.playlist;
using Txt.Core;

namespace Hls
{
    public class HlsParser
    {
        private readonly ILexer<Playlist> playlistLexer;

        public HlsParser(ILexer<Playlist> playlistLexer)
        {
            if (playlistLexer == null)
            {
                throw new ArgumentNullException(nameof(playlistLexer));
            }
            this.playlistLexer = playlistLexer;
        }

        public PlaylistFile Parse(string text, PlaylistWalker walker)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (text == string.Empty)
            {
                throw new ArgumentException("Argument is an empty string.", text);
            }
            Playlist result;
            using (var src = new StringTextSource(text))
            using (var scanner = new TextScanner(src))
            {
                result = playlistLexer.Read(scanner);
            }
            if (result == null)
            {
                throw new InvalidOperationException();
            }
            result.Walk(walker);
            return walker.Result;
        }
    }
}
