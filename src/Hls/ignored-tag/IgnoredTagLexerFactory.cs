using System;
using Txt.ABNF;
using Txt.ABNF.Core.VCHAR;
using Txt.ABNF.Core.WSP;
using Txt.Core;

namespace Hls.ignored_tag
{
    public class IgnoredTagLexerFactory : LexerFactory<IgnoredTag>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        private readonly ILexerFactory<VisibleCharacter> visibleCharacterLexerFactory;

        private readonly ILexerFactory<WhiteSpace> whiteSpaceLexerFactory;

        static IgnoredTagLexerFactory()
        {
            Default = new IgnoredTagLexerFactory(
                TerminalLexerFactory.Default,
                ConcatenationLexerFactory.Default,
                AlternationLexerFactory.Default,
                RepetitionLexerFactory.Default,
                VisibleCharacterLexerFactory.Default.Singleton(),
                WhiteSpaceLexerFactory.Default.Singleton());
        }

        public IgnoredTagLexerFactory(
            ITerminalLexerFactory terminalLexerFactory,
            IConcatenationLexerFactory concatenationLexerFactory,
            IAlternationLexerFactory alternationLexerFactory,
            IRepetitionLexerFactory repetitionLexerFactory,
            ILexerFactory<VisibleCharacter> visibleCharacterLexerFactory,
            ILexerFactory<WhiteSpace> whiteSpaceLexerFactory)
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
            if (visibleCharacterLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(visibleCharacterLexerFactory));
            }
            if (whiteSpaceLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(whiteSpaceLexerFactory));
            }
            this.terminalLexerFactory = terminalLexerFactory;
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.alternationLexerFactory = alternationLexerFactory;
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.visibleCharacterLexerFactory = visibleCharacterLexerFactory;
            this.whiteSpaceLexerFactory = whiteSpaceLexerFactory;
        }

        public static IgnoredTagLexerFactory Default { get; }

        public override ILexer<IgnoredTag> Create()
        {
            return
                new IgnoredTagLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT", StringComparer.Ordinal),
                        repetitionLexerFactory.Create(
                            alternationLexerFactory.Create(
                                visibleCharacterLexerFactory.Create(),
                                whiteSpaceLexerFactory.Create()),
                            0,
                            int.MaxValue)));
        }
    }
}
