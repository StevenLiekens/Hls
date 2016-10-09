using System;
using Hls.decimal_integer;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_VERSION
{
    public class ExtVersionLexerFactory : LexerFactory<ExtVersion>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<DecimalInteger> decimalIntegerLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static ExtVersionLexerFactory()
        {
            Default = new ExtVersionLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                DecimalIntegerLexerFactory.Default.Singleton());
        }

        public ExtVersionLexerFactory(
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

        public static ExtVersionLexerFactory Default { get; }

        public override ILexer<ExtVersion> Create()
        {
            return
                new ExtVersionLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-VERSION:", StringComparer.Ordinal),
                        decimalIntegerLexerFactory.Create()));
        }
    }
}
