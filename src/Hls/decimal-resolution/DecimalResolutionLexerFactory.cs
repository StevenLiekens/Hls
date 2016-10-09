using System;
using Hls.decimal_integer;
using Txt.Core;
using Txt.ABNF;

namespace Hls.decimal_resolution
{
    public class DecimalResolutionLexerFactory : LexerFactory<DecimalResolution>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<DecimalInteger> decimalIntegerLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static DecimalResolutionLexerFactory()
        {
            Default = new DecimalResolutionLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                DecimalIntegerLexerFactory.Default.Singleton());
        }

        public DecimalResolutionLexerFactory(
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

        public static DecimalResolutionLexerFactory Default { get; }

        public override ILexer<DecimalResolution> Create()
        {
            var decimalInteger = decimalIntegerLexerFactory.Create();
            var x = terminalLexerFactory.Create("x", StringComparer.Ordinal);
            var innerLexer = concatenationLexerFactory.Create(decimalInteger, x, decimalInteger);
            return new DecimalResolutionLexer(innerLexer);
        }
    }
}
