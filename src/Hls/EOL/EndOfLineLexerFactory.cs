using System;
using Txt.ABNF;
using Txt.ABNF.Core.CRLF;
using Txt.ABNF.Core.LF;
using Txt.Core;

namespace Hls.EOL
{
    public class EndOfLineLexerFactory : ILexerFactory<EndOfLine>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly ILexer<LineFeed> lineFeedLexer;

        private readonly ILexer<NewLine> newLineLexer;

        public EndOfLineLexerFactory(
            IAlternationLexerFactory alternationLexerFactory,
            ILexer<NewLine> newLineLexer,
            ILexer<LineFeed> lineFeedLexer)
        {
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (newLineLexer == null)
            {
                throw new ArgumentNullException(nameof(newLineLexer));
            }
            if (lineFeedLexer == null)
            {
                throw new ArgumentNullException(nameof(lineFeedLexer));
            }
            this.alternationLexerFactory = alternationLexerFactory;
            this.newLineLexer = newLineLexer;
            this.lineFeedLexer = lineFeedLexer;
        }

        public ILexer<EndOfLine> Create()
        {
            return
                new EndOfLineLexer(
                    alternationLexerFactory.Create(
                        lineFeedLexer,
                        newLineLexer));
        }
    }
}
