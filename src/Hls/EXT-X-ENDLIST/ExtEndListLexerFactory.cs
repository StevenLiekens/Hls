using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_ENDLIST
{
    public class ExtEndListLexerFactory : LexerFactory<ExtEndList>
    {
        private readonly ITerminalLexerFactory terminalLexerFactory;

        static ExtEndListLexerFactory()
        {
            Default = new ExtEndListLexerFactory(TerminalLexerFactory.Default);
        }

        public ExtEndListLexerFactory(ITerminalLexerFactory terminalLexerFactory)
        {
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            this.terminalLexerFactory = terminalLexerFactory;
        }

        public static ExtEndListLexerFactory Default { get; }

        public override ILexer<ExtEndList> Create()
        {
            return new ExtEndListLexer(terminalLexerFactory.Create("#EXT-X-ENDLIST", StringComparer.Ordinal));
        }
    }
}
