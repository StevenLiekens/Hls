using System;
using Hls.attribute_list;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_I_FRAME_STREAM_INF
{
    public class ExtIFrameStreamInfLexerFactory : LexerFactory<ExtIFrameStreamInf>
    {
        private readonly ILexerFactory<AttributeList> attributeListLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static ExtIFrameStreamInfLexerFactory()
        {
            Default = new ExtIFrameStreamInfLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                AttributeListLexerFactory.Default.Singleton());
        }

        public ExtIFrameStreamInfLexerFactory(
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

        public static ExtIFrameStreamInfLexerFactory Default { get; }

        public override ILexer<ExtIFrameStreamInf> Create()
        {
            return
                new ExtIFrameStreamInfLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXT-X-I-FRAME-STREAM-INF:", StringComparer.Ordinal),
                        attributeListLexerFactory.Create()));
        }
    }
}
