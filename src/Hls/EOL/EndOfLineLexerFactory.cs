using System;
using Txt.ABNF;
using Txt.ABNF.Core.CRLF;
using Txt.ABNF.Core.LF;
using Txt.Core;

namespace Hls.EOL
{
    public class EndOfLineLexerFactory : LexerFactory<EndOfLine>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly ILexerFactory<LineFeed> lineFeedLexerFactory;

        private readonly ILexerFactory<NewLine> newLineLexerFactory;

        static EndOfLineLexerFactory()
        {
            Default = new EndOfLineLexerFactory(
                AlternationLexerFactory.Default,
                NewLineLexerFactory.Default.Singleton(),
                LineFeedLexerFactory.Default.Singleton());
        }

        public EndOfLineLexerFactory(
            IAlternationLexerFactory alternationLexerFactory,
            ILexerFactory<NewLine> newLineLexerFactory,
            ILexerFactory<LineFeed> lineFeedLexerFactory)
        {
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (newLineLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(newLineLexerFactory));
            }
            if (lineFeedLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(lineFeedLexerFactory));
            }
            this.alternationLexerFactory = alternationLexerFactory;
            this.newLineLexerFactory = newLineLexerFactory;
            this.lineFeedLexerFactory = lineFeedLexerFactory;
        }

        public static EndOfLineLexerFactory Default { get; }

        public override ILexer<EndOfLine> Create()
        {
            return
                new EndOfLineLexer(
                    alternationLexerFactory.Create(
                        lineFeedLexerFactory.Create(),
                        newLineLexerFactory.Create()));
        }
    }
}
