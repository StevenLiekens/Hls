using System;
using Hls.decimal_integer;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_VERSION
{
    public class ExtVersionLexerFactory : ILexerFactory<ExtVersion>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexer<DecimalInteger> decimalIntegerLexer;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        public ExtVersionLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexer<DecimalInteger> decimalIntegerLexer)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (decimalIntegerLexer == null)
            {
                throw new ArgumentNullException(nameof(decimalIntegerLexer));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.decimalIntegerLexer = decimalIntegerLexer;
        }

        public ILexer<ExtVersion> Create()
        {
            return
                new ExtVersionLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-VERSION:", StringComparer.Ordinal),
                        decimalIntegerLexer));
        }
    }
}
