using System.Collections.Generic;
using Txt.Core;
using Txt.ABNF;

namespace Hls.empty
{
    public sealed class EmptyLexer : Lexer<Empty>
    {
        public override IEnumerable<Empty> Read2Impl(ITextScanner scanner, ITextContext context)
        {
            var next = scanner.Peek();
            if (next == -1)
            {
                return Empty;
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
