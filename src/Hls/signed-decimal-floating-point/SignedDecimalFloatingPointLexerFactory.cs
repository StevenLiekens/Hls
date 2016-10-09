using System;
using Hls.decimal_floating_point;
using Txt.Core;
using Txt.ABNF;

namespace Hls.signed_decimal_floating_point
{
    public class SignedDecimalFloatingPointLexerFactory : LexerFactory<SignedDecimalFloatingPoint>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<DecimalFloatingPoint> decimalFloatingPointLexerFactory;

        private readonly IOptionLexerFactory optionLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static SignedDecimalFloatingPointLexerFactory()
        {
            Default = new SignedDecimalFloatingPointLexerFactory(
                ConcatenationLexerFactory.Default,
                OptionLexerFactory.Default,
                TerminalLexerFactory.Default,
                DecimalFloatingPointLexerFactory.Default.Singleton());
        }

        public SignedDecimalFloatingPointLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            IOptionLexerFactory optionLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<DecimalFloatingPoint> decimalFloatingPointLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (optionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(optionLexerFactory));
            }
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (decimalFloatingPointLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalFloatingPointLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.optionLexerFactory = optionLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.decimalFloatingPointLexerFactory = decimalFloatingPointLexerFactory;
        }

        public static SignedDecimalFloatingPointLexerFactory Default { get; }

        public override ILexer<SignedDecimalFloatingPoint> Create()
        {
            return
                new SignedDecimalFloatingPointLexer(
                    concatenationLexerFactory.Create(
                        optionLexerFactory.Create(terminalLexerFactory.Create("-", StringComparer.Ordinal)),
                        decimalFloatingPointLexerFactory.Create()));
        }
    }
}
