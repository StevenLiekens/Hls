using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_DISCONTINUITY
{
    public class ExtDiscontinuityLexerFactory : LexerFactory<ExtDiscontinuity>
    {
        private readonly ITerminalLexerFactory terminalLexerFactory;

        public ExtDiscontinuityLexerFactory(ITerminalLexerFactory terminalLexerFactory)
        {
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            this.terminalLexerFactory = terminalLexerFactory;
        }

        public static ExtDiscontinuityLexerFactory Default { get; } =
            new ExtDiscontinuityLexerFactory(TerminalLexerFactory.Default);

        public override ILexer<ExtDiscontinuity> Create()
        {
            return
                new ExtDiscontinuityLexer(terminalLexerFactory.Create(@"#EXT-X-DISCONTINUITY", StringComparer.Ordinal));
        }
    }
}
