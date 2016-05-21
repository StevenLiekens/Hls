using System;
using Hls.attribute_list;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_I_FRAME_STREAM_INF
{
    public class ExtIFrameStreamInfLexerFactory : ILexerFactory<ExtIFrameStreamInf>
    {
        private readonly ILexer<AttributeList> attributeListLexer;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        public ExtIFrameStreamInfLexerFactory(
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

        public ILexer<ExtIFrameStreamInf> Create()
        {
            return
                new ExtIFrameStreamInfLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-I-FRAME-STREAM-INF:", StringComparer.Ordinal),
                        attributeListLexer));
        }
    }
}
