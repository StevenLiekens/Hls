using System;
using Hls.duration;
using Hls.title;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXTINF
{
    public class ExtInfLexerFactory : ILexerFactory<ExtInf>
    {
        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ILexerFactory<Duration> durationLexerFactory;

        private readonly IOptionLexerFactory optionLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        private readonly ILexerFactory<Title> titleLexerFactory;

        public ExtInfLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<Duration> durationLexerFactory,
            ILexerFactory<Title> titleLexerFactory,
            IOptionLexerFactory optionLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (durationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(durationLexerFactory));
            }
            if (titleLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(titleLexerFactory));
            }
            if (optionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(optionLexerFactory));
            }
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.durationLexerFactory = durationLexerFactory;
            this.titleLexerFactory = titleLexerFactory;
            this.optionLexerFactory = optionLexerFactory;
        }

        public ILexer<ExtInf> Create()
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
