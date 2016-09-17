using System.Collections.Generic;
using Txt.Core;
using Txt.ABNF;

namespace Hls.empty
{
    public sealed class EmptyLexer : Lexer<Empty>
    {
        protected override IEnumerable<Empty> ReadImpl(ITextScanner scanner, ITextContext context)
        {
            var next = scanner.Peek();
            if (next == -1)
            {
                return new[] { new Empty(new Terminal("", context)) };
            }
            var c = (char)next;
            if ((c == '\r') || (c == '\n'))
            {
                return new[] { new Empty(new Terminal("", context)) };
            }
            return Empty;
        }
    }
}
