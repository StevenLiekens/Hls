using System;
using Txt.Core;

namespace Hls.EOF
{
    public sealed class EndOfFileLexer : Lexer<EndOfFile>
    {
        public override ReadResult<EndOfFile> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var context = scanner.GetContext();
            var peek = scanner.Peek();
            if (peek == -1)
            {
                return ReadResult<EndOfFile>.FromResult(new EndOfFile(context));
            }
            return ReadResult<EndOfFile>.FromSyntaxError(new SyntaxError(false, "", char.ToString((char)peek), context));
        }
    }
}
