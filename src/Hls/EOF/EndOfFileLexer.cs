using Txt.Core;

namespace Hls.EOF
{
    public sealed class EndOfFileLexer : Lexer<EndOfFile>
    {
        protected override IReadResult<EndOfFile> ReadImpl(ITextScanner scanner, ITextContext context)
        {
            var peek = scanner.Peek();
            if (peek == -1)
            {
                return ReadResult<EndOfFile>.Success(new EndOfFile(context));
            }
            var syntaxError = new SyntaxError(context, "Unexpected character");
            return ReadResult<EndOfFile>.Fail(syntaxError);
        }
    }
}
