using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXTINF
{
    public sealed class ExtInfLexer : Lexer<ExtInf>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public ExtInfLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtInf> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtInf>.FromResult(new ExtInf(result.Element));
            }
            return ReadResult<ExtInf>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
