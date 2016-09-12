using System.Collections.Generic;
using Txt.Core;

namespace Hls.EOF
{
    public sealed class EndOfFileLexer : Lexer<EndOfFile>
    {
        public override IEnumerable<EndOfFile> Read2Impl(ITextScanner scanner, ITextContext context)
        {
            if (scanner.Peek() == -1)
            {
                return new[] { new EndOfFile(context) };
            }
            return Empty;
        }
    }
}
