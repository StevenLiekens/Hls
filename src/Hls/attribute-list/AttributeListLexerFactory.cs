using System;
using Hls.attribute;
using Txt.Core;
using Txt.ABNF;
using Attribute = Hls.attribute.Attribute;

namespace Hls.attribute_list
{
    public class AttributeListLexerFactory : LexerFactory<AttributeList>
    {
        private readonly ILexerFactory<Attribute> attributeLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        static AttributeListLexerFactory()
        {
            Default = new AttributeListLexerFactory(
                ConcatenationLexerFactory.Default,
                RepetitionLexerFactory.Default,
                TerminalLexerFactory.Default,
                AttributeLexerFactory.Default.Singleton());
        }

        public AttributeListLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            IRepetitionLexerFactory repetitionLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<Attribute> attributeLexerFactory)
        {
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.attributeLexerFactory = attributeLexerFactory;
        }

        public static AttributeListLexerFactory Default { get; }

        public override ILexer<AttributeList> Create()
        {
            var attribute = attributeLexerFactory.Create();
            return
                new AttributeListLexer(
                    concatenationLexerFactory.Create(
                        attribute,
                        repetitionLexerFactory.Create(
                            concatenationLexerFactory.Create(
                                terminalLexerFactory.Create(",", StringComparer.Ordinal),
                                attribute),
                            0,
                            int.MaxValue)));
        }
    }
}
