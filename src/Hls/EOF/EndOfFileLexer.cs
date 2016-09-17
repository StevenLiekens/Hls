using System.Collections.Generic;
using Txt.Core;

namespace Hls.EOF
{
    public sealed class EndOfFileLexer : Lexer<EndOfFile>
    {
        protected override IEnumerable<EndOfFile> ReadImpl(ITextScanner scanner, ITextContext context)
        {
            if (scanner.Peek() == -1)
            {
                return new[] { new EndOfFile(context) };
            }
            return Empty;
        }
    }
}
