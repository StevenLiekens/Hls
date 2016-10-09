using System;
using System.Text;
using Txt.Core;
using Txt.ABNF;
using Txt.ABNF.Core.DQUOTE;

namespace Hls.quoted_string
{
    public class QuotedStringLexerFactory : LexerFactory<QuotedString>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<DoubleQuote> doubleQuoteLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly IValueRangeLexerFactory valueRangeLexerFactory;

        static QuotedStringLexerFactory()
        {
            Default = new QuotedStringLexerFactory(
                ConcatenationLexerFactory.Default,
                RepetitionLexerFactory.Default,
                AlternationLexerFactory.Default,
                ValueRangeLexerFactory.Default,
                DoubleQuoteLexerFactory.Default.Singleton());
        }

        public QuotedStringLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            IRepetitionLexerFactory repetitionLexerFactory,
            IAlternationLexerFactory alternationLexerFactory,
            IValueRangeLexerFactory valueRangeLexerFactory,
            ILexerFactory<DoubleQuote> doubleQuoteLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
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
            if (doubleQuoteLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(doubleQuoteLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.alternationLexerFactory = alternationLexerFactory;
            this.valueRangeLexerFactory = valueRangeLexerFactory;
            this.doubleQuoteLexerFactory = doubleQuoteLexerFactory;
        }

        public static QuotedStringLexerFactory Default { get; }

        public override ILexer<QuotedString> Create()
        {
            var dquote = doubleQuoteLexerFactory.Create();
            return
                new QuotedStringLexer(
                    concatenationLexerFactory.Create(
                        dquote,
                        repetitionLexerFactory.Create(
                            alternationLexerFactory.Create(
                                valueRangeLexerFactory.Create(0x20, 0x21, Encoding.UTF8),
                                valueRangeLexerFactory.Create(0x23, 0x7E, Encoding.UTF8)),
                            0,
                            int.MaxValue),
                        dquote));
        }
    }
}
