using System;
using Hls.decimal_integer;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_DISCONTINUITY_SEQUENCE
{
    public class ExtDiscontinuitySequenceLexerFactory : LexerFactory<ExtDiscontinuitySequence>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<DecimalInteger> decimalIntegerLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static ExtDiscontinuitySequenceLexerFactory()
        {
            Default = new ExtDiscontinuitySequenceLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                DecimalIntegerLexerFactory.Default.Singleton());
        }

        public ExtDiscontinuitySequenceLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<DecimalInteger> decimalIntegerLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (decimalIntegerLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalIntegerLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.decimalIntegerLexerFactory = decimalIntegerLexerFactory;
        }

        public static ExtDiscontinuitySequenceLexerFactory Default { get; }

        public override ILexer<ExtDiscontinuitySequence> Create()
        {
            return new ExtDiscontinuitySequenceLexer(
                concatenationLexerFactory.Create(
                    terminalLexerFactory.Create(
                        @"#EXT-X-DISCONTINUITY-SEQUENCE:",
                        StringComparer.Ordinal),
                    decimalIntegerLexerFactory.Create()));
        }
    }
}
