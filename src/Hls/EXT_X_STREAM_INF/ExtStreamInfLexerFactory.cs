using System;
using Hls.attribute_list;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXT_X_STREAM_INF
{
    public class ExtStreamInfLexerFactory : ILexerFactory<ExtStreamInf>
    {
        private readonly ILexerFactory<AttributeList> attributeListLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        public ExtStreamInfLexerFactory(
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

        public ILexer<ExtStreamInf> Create()
        {
            return
                new ExtStreamInfLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-STREAM-INF:", StringComparer.Ordinal),
                        attributeListLexerFactory.Create()));
        }
    }
}
