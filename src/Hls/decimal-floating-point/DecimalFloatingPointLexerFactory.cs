using System;
using Txt.Core;
using Txt.ABNF;
using Txt.ABNF.Core.DIGIT;

namespace Hls.decimal_floating_point
{
    public class DecimalFloatingPointLexerFactory : ILexerFactory<DecimalFloatingPoint>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<Digit> digitLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        public DecimalFloatingPointLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            IRepetitionLexerFactory repetitionLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<Digit> digitLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (digitLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(digitLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.digitLexerFactory = digitLexerFactory;
        }

        public ILexer<DecimalFloatingPoint> Create()
        {
            var digit = digitLexerFactory.Create();
            var separator = terminalLexerFactory.Create(".", StringComparer.Ordinal);
            var integers = repetitionLexerFactory.Create(digit, 0, int.MaxValue);
            var decimals = repetitionLexerFactory.Create(digit, 1, int.MaxValue);
            var innerLexer = concatenationLexerFactory.Create(integers, separator, decimals);
            return new DecimalFloatingPointLexer(innerLexer);
        }
    }
}
