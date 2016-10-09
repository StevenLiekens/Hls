using System;
using Hls.attribute_name;
using Hls.attribute_value;
using Txt.ABNF;
using Txt.Core;

namespace Hls.attribute
{
    public class AttributeLexerFactory : LexerFactory<Attribute>
    {
        private readonly ILexerFactory<AttributeName> attributeNameLexerFactory;

        private readonly ILexerFactory<AttributeValue> attributeValueLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static AttributeLexerFactory()
        {
            Default = new AttributeLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                AttributeNameLexerFactory.Default.Singleton(),
                AttributeValueLexerFactory.Default.Singleton());
        }

        public AttributeLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<AttributeName> attributeNameLexerFactory,
            ILexerFactory<AttributeValue> attributeValueLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (attributeNameLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(attributeNameLexerFactory));
            }
            if (attributeValueLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(attributeValueLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.attributeNameLexerFactory = attributeNameLexerFactory;
            this.attributeValueLexerFactory = attributeValueLexerFactory;
        }

        public static AttributeLexerFactory Default { get; }

        public override ILexer<Attribute> Create()
        {
            return
                new AttributeLexer(
                    concatenationLexerFactory.Create(
                        attributeNameLexerFactory.Create(),
                        terminalLexerFactory.Create("=", StringComparer.Ordinal),
                        attributeValueLexerFactory.Create()));
        }
    }
}
