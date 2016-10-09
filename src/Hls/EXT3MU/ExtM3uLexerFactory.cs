using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXTM3U
{
    public class ExtM3uLexerFactory : LexerFactory<ExtM3u>
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

        public static ExtM3uLexerFactory Default { get; } = new ExtM3uLexerFactory(TerminalLexerFactory.Default);

        public override ILexer<ExtM3u> Create()
        {
            return new ExtM3uLexer(terminalLexerFactory.Create("#EXTM3U", StringComparer.Ordinal));
        }
    }
}
