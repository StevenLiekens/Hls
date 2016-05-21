using System;
using Txt.ABNF;
using Txt.ABNF.Core.VCHAR;
using Txt.ABNF.Core.WSP;
using Txt.Core;

namespace Hls.ignored_tag
{
    public class IgnoredTagLexerFactory : ILexerFactory<IgnoredTag>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        private readonly ILexer<VisibleCharacter> visibleCharacterLexer;

        private readonly ILexer<WhiteSpace> whiteSpaceLexer;

        public IgnoredTagLexerFactory(
            ITerminalLexerFactory terminalLexerFactory,
            IConcatenationLexerFactory concatenationLexerFactory,
            IAlternationLexerFactory alternationLexerFactory,
            IRepetitionLexerFactory repetitionLexerFactory,
            ILexer<VisibleCharacter> visibleCharacterLexer,
            ILexer<WhiteSpace> whiteSpaceLexer)
        {
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }
            if (visibleCharacterLexer == null)
            {
                throw new ArgumentNullException(nameof(visibleCharacterLexer));
            }
            if (whiteSpaceLexer == null)
            {
                throw new ArgumentNullException(nameof(whiteSpaceLexer));
            }
            this.terminalLexerFactory = terminalLexerFactory;
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.alternationLexerFactory = alternationLexerFactory;
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.visibleCharacterLexer = visibleCharacterLexer;
            this.whiteSpaceLexer = whiteSpaceLexer;
        }

        public ILexer<IgnoredTag> Create()
        {
            return
                new IgnoredTagLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT", StringComparer.Ordinal),
                        repetitionLexerFactory.Create(
                            alternationLexerFactory.Create(visibleCharacterLexer, whiteSpaceLexer),
                            0,
                            int.MaxValue)));
        }
    }
}
