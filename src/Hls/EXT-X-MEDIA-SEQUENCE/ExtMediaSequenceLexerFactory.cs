using System;
using Hls.decimal_integer;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXT_X_MEDIA_SEQUENCE
{
    public class ExtMediaSequenceLexerFactory : LexerFactory<ExtMediaSequence>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<DecimalInteger> decimalIntegerLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static ExtMediaSequenceLexerFactory()
        {
            Default = new ExtMediaSequenceLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                DecimalIntegerLexerFactory.Default.Singleton());
        }

        public ExtMediaSequenceLexerFactory(
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

        public static ExtMediaSequenceLexerFactory Default { get; }

        public override ILexer<ExtMediaSequence> Create()
        {
            return
                new ExtMediaSequenceLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-MEDIA-SEQUENCE:", StringComparer.Ordinal),
                        decimalIntegerLexerFactory.Create()));
        }
    }
}
