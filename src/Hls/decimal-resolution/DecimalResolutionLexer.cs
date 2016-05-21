using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.decimal_resolution
{
    public sealed class DecimalResolutionLexer : Lexer<DecimalResolution>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public DecimalResolutionLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<DecimalResolution> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<DecimalResolution>.FromResult(new DecimalResolution(result.Element));
            }
            return
                ReadResult<DecimalResolution>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
