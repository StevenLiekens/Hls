using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_DISCONTINUITY
{
    public sealed class ExtDiscontinuityLexer : Lexer<ExtDiscontinuity>
    {
        private readonly ILexer<Terminal> innerLexer;

        public ExtDiscontinuityLexer(ILexer<Terminal> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtDiscontinuity> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtDiscontinuity>.FromResult(new ExtDiscontinuity(result.Element));
            }
            return ReadResult<ExtDiscontinuity>.FromSyntaxError(
                SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
