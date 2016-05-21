using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_ENDLIST
{
    public class ExtEndListLexerFactory : ILexerFactory<ExtEndList>
    {
        private readonly ITerminalLexerFactory terminalLexerFactory;

        public ExtEndListLexerFactory(ITerminalLexerFactory terminalLexerFactory)
        {
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            this.terminalLexerFactory = terminalLexerFactory;
        }

        public ILexer<ExtEndList> Create()
        {
            return new ExtEndListLexer(terminalLexerFactory.Create("#EXT-X-ENDLIST", StringComparer.Ordinal));
        }
    }
}
