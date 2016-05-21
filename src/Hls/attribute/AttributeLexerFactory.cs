using System;
using Hls.attribute_name;
using Hls.attribute_value;
using Txt.ABNF;
using Txt.Core;

namespace Hls.attribute
{
    public class AttributeLexerFactory : ILexerFactory<Attribute>
    {
        private readonly ILexer<AttributeName> attributeNameLexer;

        private readonly ILexer<AttributeValue> attributeValueLexer;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        public AttributeLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexer<AttributeName> attributeNameLexer,
            ILexer<AttributeValue> attributeValueLexer)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (attributeNameLexer == null)
            {
                throw new ArgumentNullException(nameof(attributeNameLexer));
            }
            if (attributeValueLexer == null)
            {
                throw new ArgumentNullException(nameof(attributeValueLexer));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.attributeNameLexer = attributeNameLexer;
            this.attributeValueLexer = attributeValueLexer;
        }

        public ILexer<Attribute> Create()
        {
            return
                new AttributeLexer(
                    concatenationLexerFactory.Create(
                        attributeNameLexer,
                        terminalLexerFactory.Create("=", StringComparer.Ordinal),
                        attributeValueLexer));
        }
    }
}
