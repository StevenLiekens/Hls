using Txt.Core;
using Txt.ABNF;

namespace Hls.empty
{
    public sealed class EmptyLexer : Lexer<Empty>
    {
        protected override IReadResult<Empty> ReadImpl(ITextScanner scanner, ITextContext context)
        {
            var next = scanner.Peek();
            if (next == -1)
            {
                return ReadResult<Empty>.Success(new Empty(new Terminal("", context)));
            }
            var c = (char)next;
            if ((c == '\r') || (c == '\n'))
            {
                return ReadResult<Empty>.Success(new Empty(new Terminal("", context)));
            }
            var syntaxError = new SyntaxError(context, "Unexpected character");
            return ReadResult<Empty>.Fail(syntaxError);
        }
    }
}
