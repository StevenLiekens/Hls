using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXT_X_STREAM_INF
{
    public sealed class ExtStreamInfLexer : Lexer<ExtStreamInf>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public ExtStreamInfLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtStreamInf> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtStreamInf>.FromResult(new ExtStreamInf(result.Element));
            }
            return ReadResult<ExtStreamInf>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
