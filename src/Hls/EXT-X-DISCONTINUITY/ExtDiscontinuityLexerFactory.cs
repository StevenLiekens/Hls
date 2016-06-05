using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_DISCONTINUITY
{
    public class ExtDiscontinuityLexerFactory : ILexerFactory<ExtDiscontinuity>
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

        public ILexer<ExtDiscontinuity> Create()
        {
            return
                new ExtDiscontinuityLexer(terminalLexerFactory.Create(@"#EXT-X-DISCONTINUITY", StringComparer.Ordinal));
        }
    }
}
