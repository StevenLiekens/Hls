using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.signed_decimal_floating_point
{
    public sealed class SignedDecimalFloatingPointLexer : Lexer<SignedDecimalFloatingPoint>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public SignedDecimalFloatingPointLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<SignedDecimalFloatingPoint> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<SignedDecimalFloatingPoint>.FromResult(new SignedDecimalFloatingPoint(result.Element));
            }
            return
                ReadResult<SignedDecimalFloatingPoint>.FromSyntaxError(
                    SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
