using System;
using Hls.attribute_list;
using Hls.EOL;
using Txt.ABNF;
using Txt.Core;
using UriSyntax.URI_reference;

namespace Hls.EXT_X_STREAM_INF
{
    public class ExtStreamInfLexerFactory : LexerFactory<ExtStreamInf>
    {
        private readonly ILexerFactory<AttributeList> attributeListLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<EndOfLine> endOfLineLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        private readonly ILexerFactory<UriReference> uriReferenceLexerFactory;

        static ExtStreamInfLexerFactory()
        {
            Default = new ExtStreamInfLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                AttributeListLexerFactory.Default.Singleton(),
                EndOfLineLexerFactory.Default.Singleton(),
                UriReferenceLexerFactory.Default.Singleton());
        }

        public ExtStreamInfLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<AttributeList> attributeListLexerFactory,
            ILexerFactory<EndOfLine> endOfLineLexerFactory,
            ILexerFactory<UriReference> uriReferenceLexerFactory)
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
            if (endOfLineLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(endOfLineLexerFactory));
            }
            if (uriReferenceLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(uriReferenceLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.attributeListLexerFactory = attributeListLexerFactory;
            this.endOfLineLexerFactory = endOfLineLexerFactory;
            this.uriReferenceLexerFactory = uriReferenceLexerFactory;
        }

        public static ExtStreamInfLexerFactory Default { get; }

        public override ILexer<ExtStreamInf> Create()
        {
            return
                new ExtStreamInfLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-STREAM-INF:", StringComparer.Ordinal),
                        attributeListLexerFactory.Create(),
                        endOfLineLexerFactory.Create(),
                        uriReferenceLexerFactory.Create()));
        }
    }
}
