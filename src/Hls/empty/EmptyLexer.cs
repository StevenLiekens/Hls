using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.empty
{
    public sealed class EmptyLexer : Lexer<Empty>
    {
        public override ReadResult<Empty> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            if (scanner.EndOfInput)
            {
                return ReadResult<Empty>.FromResult(new Empty(new Terminal("", scanner.GetContext())));
            }
            if (scanner.TryMatch("\r\n").Success)
            {
                scanner.Unread("\r\n");
                return ReadResult<Empty>.FromResult(new Empty(new Terminal("", scanner.GetContext())));
            }
            var matchResult = scanner.TryMatch("\n");
            if (matchResult.Success)
            {
                scanner.Unread("\n");
                return ReadResult<Empty>.FromResult(new Empty(new Terminal("", scanner.GetContext())));
            }
            return ReadResult<Empty>.FromSyntaxError(new SyntaxError(false, "", matchResult.Text, scanner.GetContext()));
        }
    }
}
