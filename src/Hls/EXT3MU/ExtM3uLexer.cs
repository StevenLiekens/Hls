using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXTM3U
{
    public sealed class ExtM3uLexer : Lexer<ExtM3u>
    {
        private readonly ILexer<Terminal> innerLexer;

        public ExtM3uLexer(ILexer<Terminal> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtM3u> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtM3u>.FromResult(new ExtM3u(result.Element));
            }
            return ReadResult<ExtM3u>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
