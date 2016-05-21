using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.line
{
    public sealed class LineLexer : Lexer<Line>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public LineLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<Line> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<Line>.FromResult(new Line(result.Element));
            }
            return ReadResult<Line>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
