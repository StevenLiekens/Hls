using System;
using Hls.decimal_integer;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXT_X_TARGETDURATION
{
    public class ExtTargetDurationLexerFactory : LexerFactory<ExtTargetDuration>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<DecimalInteger> decimalIntegerLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static ExtTargetDurationLexerFactory()
        {
            Default = new ExtTargetDurationLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                DecimalIntegerLexerFactory.Default.Singleton());
        }

        public ExtTargetDurationLexerFactory(
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

        public static ExtTargetDurationLexerFactory Default { get; }

        public override ILexer<ExtTargetDuration> Create()
        {
            return
                new ExtTargetDurationLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-TARGETDURATION:", StringComparer.Ordinal),
                        decimalIntegerLexerFactory.Create()));
        }
    }
}
