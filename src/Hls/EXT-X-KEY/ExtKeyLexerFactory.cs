using System;
using Hls.attribute_list;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_KEY
{
    public class ExtKeyLexerFactory : ILexerFactory<ExtKey>
    {
        private readonly ILexer<AttributeList> attributeListLexer;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        public ExtKeyLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexer<AttributeList> attributeListLexer)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (attributeListLexer == null)
            {
                throw new ArgumentNullException(nameof(attributeListLexer));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.attributeListLexer = attributeListLexer;
        }

        public ILexer<ExtKey> Create()
        {
            return
                new ExtKeyLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-KEY:", StringComparer.Ordinal),
                        attributeListLexer));
        }
    }
}
