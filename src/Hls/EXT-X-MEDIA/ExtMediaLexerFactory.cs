using System;
using Hls.attribute_list;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_MEDIA
{
    public class ExtMediaLexerFactory : ILexerFactory<ExtMedia>
    {
        private readonly ILexer<AttributeList> attributeListLexer;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        public ExtMediaLexerFactory(
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

        public ILexer<ExtMedia> Create()
        {
            return
                new ExtMediaLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-MEDIA:", StringComparer.Ordinal),
                        attributeListLexer));
        }
    }
}
