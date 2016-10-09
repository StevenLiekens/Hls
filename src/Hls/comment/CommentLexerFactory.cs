using System;
using System.Text;
using Txt.Core;
using Txt.ABNF;
using Txt.ABNF.Core.VCHAR;
using Txt.ABNF.Core.WSP;

namespace Hls.comment
{
    public class CommentLexerFactory : LexerFactory<Comment>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly IOptionLexerFactory optionLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        private readonly IValueRangeLexerFactory valueRangeLexerFactory;

        private readonly ILexerFactory<VisibleCharacter> visibleCharacterLexerFactory;

        private readonly ILexerFactory<WhiteSpace> whiteSpaceLexerFactory;

        static CommentLexerFactory()
        {
            Default = new CommentLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                RepetitionLexerFactory.Default,
                AlternationLexerFactory.Default,
                ValueRangeLexerFactory.Default,
                OptionLexerFactory.Default,
                VisibleCharacterLexerFactory.Default.Singleton(),
                WhiteSpaceLexerFactory.Default.Singleton());
        }

        public CommentLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            IRepetitionLexerFactory repetitionLexerFactory,
            IAlternationLexerFactory alternationLexerFactory,
            IValueRangeLexerFactory valueRangeLexerFactory,
            IOptionLexerFactory optionLexerFactory,
            ILexerFactory<VisibleCharacter> visibleCharacterLexerFactory,
            ILexerFactory<WhiteSpace> whiteSpaceLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (valueRangeLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(valueRangeLexerFactory));
            }
            if (optionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(optionLexerFactory));
            }
            if (visibleCharacterLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(visibleCharacterLexerFactory));
            }
            if (whiteSpaceLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(whiteSpaceLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.alternationLexerFactory = alternationLexerFactory;
            this.valueRangeLexerFactory = valueRangeLexerFactory;
            this.optionLexerFactory = optionLexerFactory;
            this.visibleCharacterLexerFactory = visibleCharacterLexerFactory;
            this.whiteSpaceLexerFactory = whiteSpaceLexerFactory;
        }

        public static CommentLexerFactory Default { get; }

        public override ILexer<Comment> Create()
        {
            var octo = terminalLexerFactory.Create("#", StringComparer.Ordinal);
            var vchar = visibleCharacterLexerFactory.Create();
            var wsp = whiteSpaceLexerFactory.Create();

            // VCHAR or WSP characters
            var characters = repetitionLexerFactory.Create(alternationLexerFactory.Create(vchar, wsp), 0, int.MaxValue);

            // "EX"
            var ex = terminalLexerFactory.Create("EX", StringComparer.Ordinal);

            // VCHAR except T, or WSP
            var beforeT = valueRangeLexerFactory.Create(0x21, 0x53, Encoding.UTF8);
            var afterT = valueRangeLexerFactory.Create(0x55, 0x7E, Encoding.UTF8);
            var notT = alternationLexerFactory.Create(beforeT, afterT, wsp);

            // "E"
            var e = terminalLexerFactory.Create("E", StringComparer.Ordinal);

            // VCHAR except X, or WSP
            var beforeX = valueRangeLexerFactory.Create(0x21, 0x57, Encoding.UTF8);
            var afterX = valueRangeLexerFactory.Create(0x59, 0x7E, Encoding.UTF8);
            var notX = alternationLexerFactory.Create(beforeX, afterX, wsp);

            // VCHAR except E, or WSP
            var beforeE = valueRangeLexerFactory.Create(0x21, 0x44, Encoding.UTF8);
            var afterE = valueRangeLexerFactory.Create(0x46, 0x7E, Encoding.UTF8);
            var notE = alternationLexerFactory.Create(beforeE, afterE, wsp);
            var safetyCheck = alternationLexerFactory.Create(
                concatenationLexerFactory.Create(ex, notT),
                concatenationLexerFactory.Create(e, notX),
                notE);
            var innerLexer = concatenationLexerFactory.Create(
                octo,
                optionLexerFactory.Create(concatenationLexerFactory.Create(safetyCheck, characters)));
            return new CommentLexer(innerLexer);
        }
    }
}
