using System;
using Hls.EOL;
using Hls.EXTM3U;
using Hls.line;
using Txt.ABNF;
using Txt.Core;

namespace Hls.playlist
{
    public class PlaylistLexerFactory : LexerFactory<Playlist>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<EndOfLine> endOfLineLexerFactory;

        private readonly ILexerFactory<ExtM3u> extM3ULexerFactory;

        private readonly ILexerFactory<Line> lineLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        static PlaylistLexerFactory()
        {
            Default = new PlaylistLexerFactory(
                ConcatenationLexerFactory.Default,
                RepetitionLexerFactory.Default,
                ExtM3uLexerFactory.Default.Singleton(),
                EndOfLineLexerFactory.Default.Singleton(),
                LineLexerFactory.Default.Singleton());
        }

        public PlaylistLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            IRepetitionLexerFactory repetitionLexerFactory,
            ILexerFactory<ExtM3u> extM3ULexerFactory,
            ILexerFactory<EndOfLine> endOfLineLexerFactory,
            ILexerFactory<Line> lineLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }
            if (extM3ULexerFactory == null)
            {
                throw new ArgumentNullException(nameof(extM3ULexerFactory));
            }
            if (endOfLineLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(endOfLineLexerFactory));
            }
            if (lineLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(lineLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.extM3ULexerFactory = extM3ULexerFactory;
            this.endOfLineLexerFactory = endOfLineLexerFactory;
            this.lineLexerFactory = lineLexerFactory;
        }

        public static PlaylistLexerFactory Default { get; }

        public override ILexer<Playlist> Create()
        {
            return
                new PlaylistLexer(
                    concatenationLexerFactory.Create(
                        extM3ULexerFactory.Create(),
                        endOfLineLexerFactory.Create(),
                        repetitionLexerFactory.Create(lineLexerFactory.Create(), 0, int.MaxValue)));
        }
    }
}
