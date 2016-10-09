using System;
using Hls.decimal_floating_point;
using Hls.decimal_integer;
using Txt.Core;
using Txt.ABNF;

namespace Hls.duration
{
    public class DurationLexerFactory : LexerFactory<Duration>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly ILexerFactory<DecimalFloatingPoint> decimalFloatingPointLexerFactory;

        private readonly ILexerFactory<DecimalInteger> decimalIntegerLexerFactory;

        static DurationLexerFactory()
        {
            Default = new DurationLexerFactory(
                AlternationLexerFactory.Default,
                DecimalFloatingPointLexerFactory.Default.Singleton(),
                DecimalIntegerLexerFactory.Default.Singleton());
        }

        public DurationLexerFactory(
            IAlternationLexerFactory alternationLexerFactory,
            ILexerFactory<DecimalFloatingPoint> decimalFloatingPointLexerFactory,
            ILexerFactory<DecimalInteger> decimalIntegerLexerFactory)
        {
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (decimalFloatingPointLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalFloatingPointLexerFactory));
            }
            if (decimalIntegerLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalIntegerLexerFactory));
            }
            this.alternationLexerFactory = alternationLexerFactory;
            this.decimalFloatingPointLexerFactory = decimalFloatingPointLexerFactory;
            this.decimalIntegerLexerFactory = decimalIntegerLexerFactory;
        }

        public static DurationLexerFactory Default { get; }

        public override ILexer<Duration> Create()
        {
            return
                new DurationLexer(
                    alternationLexerFactory.Create(
                        decimalFloatingPointLexerFactory.Create(),
                        decimalIntegerLexerFactory.Create()));
        }
    }
}
