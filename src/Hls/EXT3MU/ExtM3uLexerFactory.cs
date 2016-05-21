using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXTM3U
{
    public class ExtM3uLexerFactory : ILexerFactory<ExtM3u>
    {
        private readonly ITerminalLexerFactory terminalLexerFactory;

        public ExtM3uLexerFactory(ITerminalLexerFactory terminalLexerFactory)
        {
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            this.terminalLexerFactory = terminalLexerFactory;
        }

        public ILexer<ExtM3u> Create()
        {
            return new ExtM3uLexer(terminalLexerFactory.Create("#EXTM3U", StringComparer.Ordinal));
        }
    }
}
