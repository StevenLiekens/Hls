using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EOL
{
    public sealed class EndOfLineLexer : Lexer<EndOfLine>
    {
        private readonly ILexer<Alternation> innerLexer;

        public EndOfLineLexer(ILexer<Alternation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<EndOfLine> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<EndOfLine>.FromResult(new EndOfLine(result.Element));
            }
            return ReadResult<EndOfLine>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
