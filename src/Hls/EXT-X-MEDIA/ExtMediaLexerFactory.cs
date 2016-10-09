using System;
using Hls.attribute_list;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_MEDIA
{
    public class ExtMediaLexerFactory : LexerFactory<ExtMedia>
    {
        private readonly ILexerFactory<AttributeList> attributeListLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static ExtMediaLexerFactory()
        {
            Default = new ExtMediaLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                AttributeListLexerFactory.Default.Singleton());
        }

        public ExtMediaLexerFactory(
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

        public static ExtMediaLexerFactory Default { get; }

        public override ILexer<ExtMedia> Create()
        {
            return
                new ExtMediaLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-MEDIA:", StringComparer.Ordinal),
                        attributeListLexerFactory.Create()));
        }
    }
}
