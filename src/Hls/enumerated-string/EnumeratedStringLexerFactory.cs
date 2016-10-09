using System;
using System.Text;
using Txt.ABNF;
using Txt.Core;

namespace Hls.enumerated_string
{
    public class EnumeratedStringLexerFactory : LexerFactory<EnumeratedString>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly IValueRangeLexerFactory valueRangeLexerFactory;

        static EnumeratedStringLexerFactory()
        {
            Default = new EnumeratedStringLexerFactory(
                RepetitionLexerFactory.Default,
                AlternationLexerFactory.Default,
                ValueRangeLexerFactory.Default);
        }

        public EnumeratedStringLexerFactory(
            IRepetitionLexerFactory repetitionLexerFactory,
            IAlternationLexerFactory alternationLexerFactory,
            IValueRangeLexerFactory valueRangeLexerFactory)
        {
            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (valueRangeLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(valueRangeLexerFactory));
            }
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.alternationLexerFactory = alternationLexerFactory;
            this.valueRangeLexerFactory = valueRangeLexerFactory;
        }

        public static EnumeratedStringLexerFactory Default { get; }

        public override ILexer<EnumeratedString> Create()
        {
            return
                new EnumeratedStringLexer(
                    repetitionLexerFactory.Create(
                        alternationLexerFactory.Create(
                            valueRangeLexerFactory.Create(0x21, 0x21, Encoding.UTF8),
                            valueRangeLexerFactory.Create(0x23, 0x2B, Encoding.UTF8),
                            valueRangeLexerFactory.Create(0x2D, 0x7E, Encoding.UTF8)),
                        1,
                        int.MaxValue));
        }
    }
}
