using System;
using Hls.attribute_list;
using Hls.EOL;
using Txt.ABNF;
using Txt.Core;
using UriSyntax.URI_reference;

namespace Hls.EXT_X_STREAM_INF
{
    public class ExtStreamInfLexerFactory : ILexerFactory<ExtStreamInf>
    {
        private readonly ILexerFactory<AttributeList> attributeListLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexer<EndOfLine> endOfLineLexer;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        private readonly ILexer<UriReference> uriReferenceLexer;

        public ExtStreamInfLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<AttributeList> attributeListLexerFactory,
            ILexer<EndOfLine> endOfLineLexer,
            ILexer<UriReference> uriReferenceLexer)
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
            if (endOfLineLexer == null)
            {
                throw new ArgumentNullException(nameof(endOfLineLexer));
            }
            if (uriReferenceLexer == null)
            {
                throw new ArgumentNullException(nameof(uriReferenceLexer));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.attributeListLexerFactory = attributeListLexerFactory;
            this.endOfLineLexer = endOfLineLexer;
            this.uriReferenceLexer = uriReferenceLexer;
        }

        public ILexer<ExtStreamInf> Create()
        {
            return
                new ExtStreamInfLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-STREAM-INF:", StringComparer.Ordinal),
                        attributeListLexerFactory.Create(),
                        endOfLineLexer,
                        uriReferenceLexer));
        }
    }
}
