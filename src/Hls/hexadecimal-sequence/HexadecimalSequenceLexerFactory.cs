using System;
using Txt.Core;
using Txt.ABNF;
using Txt.ABNF.Core.HEXDIG;

namespace Hls.hexadecimal_sequence
{
    public class HexadecimalSequenceLexerFactory : LexerFactory<HexadecimalSequence>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<HexadecimalDigit> hexadecimalDigitLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static HexadecimalSequenceLexerFactory()
        {
            Default = new HexadecimalSequenceLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                RepetitionLexerFactory.Default,
                HexadecimalDigitLexerFactory.Default.Singleton());
        }

        public HexadecimalSequenceLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            IRepetitionLexerFactory repetitionLexerFactory,
            ILexerFactory<HexadecimalDigit> hexadecimalDigitLexerFactory)
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
            if (hexadecimalDigitLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(hexadecimalDigitLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.hexadecimalDigitLexerFactory = hexadecimalDigitLexerFactory;
        }

        public static HexadecimalSequenceLexerFactory Default { get; }

        public override ILexer<HexadecimalSequence> Create()
        {
            return
                new HexadecimalSequenceLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("0x", StringComparer.OrdinalIgnoreCase),
                        repetitionLexerFactory.Create(hexadecimalDigitLexerFactory.Create(), 1, int.MaxValue)));
        }
    }
}
