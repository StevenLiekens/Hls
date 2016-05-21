using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.decimal_floating_point
{
    public sealed class DecimalFloatingPointLexer : Lexer<DecimalFloatingPoint>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public DecimalFloatingPointLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<DecimalFloatingPoint> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<DecimalFloatingPoint>.FromResult(new DecimalFloatingPoint(result.Element));
            }
            return
                ReadResult<DecimalFloatingPoint>.FromSyntaxError(
                    SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
