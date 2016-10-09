using System;
using Hls.duration;
using Hls.title;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXTINF
{
    public class ExtInfLexerFactory : LexerFactory<ExtInf>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<Duration> durationLexerFactory;

        private readonly IOptionLexerFactory optionLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        private readonly ILexerFactory<Title> titleLexerFactory;

        static ExtInfLexerFactory()
        {
            Default = new ExtInfLexerFactory(
                ConcatenationLexerFactory.Default,
                TerminalLexerFactory.Default,
                OptionLexerFactory.Default,
                DurationLexerFactory.Default.Singleton(),
                TitleLexerFactory.Default.Singleton());
        }

        public ExtInfLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            IOptionLexerFactory optionLexerFactory,
            ILexerFactory<Duration> durationLexerFactory,
            ILexerFactory<Title> titleLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (optionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(optionLexerFactory));
            }
            if (durationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(durationLexerFactory));
            }
            if (titleLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(titleLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.optionLexerFactory = optionLexerFactory;
            this.durationLexerFactory = durationLexerFactory;
            this.titleLexerFactory = titleLexerFactory;
        }

        public static ExtInfLexerFactory Default { get; }

        public override ILexer<ExtInf> Create()
        {
            return
                new ExtInfLexer(
                    concatenationLexerFactory.Create(
                        terminalLexerFactory.Create("#EXTINF:", StringComparer.Ordinal),
                        durationLexerFactory.Create(),
                        terminalLexerFactory.Create(",", StringComparer.Ordinal),
                        optionLexerFactory.Create(titleLexerFactory.Create())));
        }
    }
}
