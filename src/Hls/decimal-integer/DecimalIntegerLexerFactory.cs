using System;
using Txt.Core;
using Txt.ABNF;
using Txt.ABNF.Core.DIGIT;

namespace Hls.decimal_integer
{
    public class DecimalIntegerLexerFactory : LexerFactory<DecimalInteger>
    {
        private readonly ILexerFactory<Digit> digitLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        static DecimalIntegerLexerFactory()
        {
            Default = new DecimalIntegerLexerFactory(
                RepetitionLexerFactory.Default,
                DigitLexerFactory.Default.Singleton());
        }

        public DecimalIntegerLexerFactory(
            IRepetitionLexerFactory repetitionLexerFactory,
            ILexerFactory<Digit> digitLexerFactory)
        {
            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }
            if (digitLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(digitLexerFactory));
            }
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.digitLexerFactory = digitLexerFactory;
        }

        public static DecimalIntegerLexerFactory Default { get; }

        public override ILexer<DecimalInteger> Create()
        {
            return new DecimalIntegerLexer(repetitionLexerFactory.Create(digitLexerFactory.Create(), 1, 20));
        }
    }
}
