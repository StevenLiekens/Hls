using Txt.Core;
using Txt.ABNF;

namespace Hls.empty
{
    public sealed class EmptyLexer : Lexer<Empty>
    {
        protected override ReadResult<Empty> ReadImpl(ITextScanner scanner, ITextContext context)
        {
            var peek = scanner.Peek();
            if (peek == -1)
            {
                return new ReadResult<Empty>(new Empty(new Terminal("", context)));
            }
            var c = (char)peek;
            if (c == '\r')
            {
                return new ReadResult<Empty>(new Empty(new Terminal("", context)));
            }
            if (c == '\n')
            {
                return new ReadResult<Empty>(new Empty(new Terminal("", context)));
            }
            return new ReadResult<Empty>(new SyntaxError(false, "", char.ToString(c), context));
        }
    }
}
