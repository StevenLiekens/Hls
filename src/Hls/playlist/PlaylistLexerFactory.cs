using System;
using Hls.EOF;
using Hls.EOL;
using Hls.EXTM3U;
using Hls.line;
using Txt.ABNF;
using Txt.Core;

namespace Hls.playlist
{
    public class PlaylistLexerFactory : ILexerFactory<Playlist>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexer<EndOfFile> endOfFileLexer;

        private readonly ILexer<EndOfLine> endOfLineLexer;

        private readonly ILexer<ExtM3u> extM3ULexer;

        private readonly ILexer<Line> entryLexer;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        public PlaylistLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ILexer<ExtM3u> extM3ULexer,
            ILexer<EndOfLine> endOfLineLexer,
            IRepetitionLexerFactory repetitionLexerFactory,
            ILexer<Line> entryLexer,
            ILexer<EndOfFile> endOfFileLexer)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (extM3ULexer == null)
            {
                throw new ArgumentNullException(nameof(extM3ULexer));
            }
            if (endOfLineLexer == null)
            {
                throw new ArgumentNullException(nameof(endOfLineLexer));
            }
            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }
            if (entryLexer == null)
            {
                throw new ArgumentNullException(nameof(entryLexer));
            }
            if (endOfFileLexer == null)
            {
                throw new ArgumentNullException(nameof(endOfFileLexer));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.extM3ULexer = extM3ULexer;
            this.endOfLineLexer = endOfLineLexer;
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.entryLexer = entryLexer;
            this.endOfFileLexer = endOfFileLexer;
        }

        public ILexer<Playlist> Create()
        {
            return
                new PlaylistLexer(
                    concatenationLexerFactory.Create(
                        extM3ULexer,
                        endOfLineLexer,
                        repetitionLexerFactory.Create(entryLexer, 0, int.MaxValue),
                        endOfFileLexer));
        }
    }
}
