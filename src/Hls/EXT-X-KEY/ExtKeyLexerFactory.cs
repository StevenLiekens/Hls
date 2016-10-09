using System;
using Hls.attribute_list;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_KEY
{
    public class ExtKeyLexerFactory : LexerFactory<ExtKey>
    {
        private readonly ILexerFactory<AttributeList> attributeListLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static ExtKeyLexerFactory()
        {
            Default = new ExtKeyLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                AttributeListLexerFactory.Default.Singleton());
        }

        public ExtKeyLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<AttributeList> attributeListLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (attributeListLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(attributeListLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.attributeListLexerFactory = attributeListLexerFactory;
        }

        public static ExtKeyLexerFactory Default { get; }

        public override ILexer<ExtKey> Create()
        {
            return
                new ExtKeyLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-KEY:", StringComparer.Ordinal),
                        attributeListLexerFactory.Create()));
        }
    }
}
