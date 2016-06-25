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
                return new ReadResult<EndOfFile>(new EndOfFile(context));
            }
            return new ReadResult<EndOfFile>(new SyntaxError(false, "", char.ToString((char)peek), context));
        }
    }
}
