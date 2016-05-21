using System;
using System.Text;
using Txt.Core;
using Txt.ABNF;
using Txt.ABNF.Core.DIGIT;

namespace Hls.attribute_name
{
    public class AttributeNameLexerFactory : ILexerFactory<AttributeName>
    {
        private readonly IAlternationLexerFactory alternationLexerFactory;

        private readonly ILexerFactory<Digit> digitLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        private readonly IValueRangeLexerFactory valueRangeLexerFactory;

        public AttributeNameLexerFactory(
            IRepetitionLexerFactory repetitionLexerFactory,
            IAlternationLexerFactory alternationLexerFactory,
            IValueRangeLexerFactory valueRangeLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<Digit> digitLexerFactory)
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
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (digitLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(digitLexerFactory));
            }
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.alternationLexerFactory = alternationLexerFactory;
            this.valueRangeLexerFactory = valueRangeLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.digitLexerFactory = digitLexerFactory;
        }

        public ILexer<AttributeName> Create()
        {
            return
                new AttributeNameLexer(
                    repetitionLexerFactory.Create(
                        alternationLexerFactory.Create(
                            valueRangeLexerFactory.Create(0x41, 0x5A, Encoding.UTF8),
                            digitLexerFactory.Create(),
                            terminalLexerFactory.Create("-", StringComparer.Ordinal)),
                        1,
                        int.MaxValue));
        }
    }
}
