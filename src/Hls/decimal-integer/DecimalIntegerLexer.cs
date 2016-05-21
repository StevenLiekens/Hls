using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.decimal_integer
{
    public sealed class DecimalIntegerLexer : Lexer<DecimalInteger>
    {
        private readonly ILexer<Repetition> innerLexer;

        public DecimalIntegerLexer(ILexer<Repetition> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<DecimalInteger> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<DecimalInteger>.FromResult(new DecimalInteger(result.Element));
            }
            return ReadResult<DecimalInteger>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
