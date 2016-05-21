using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_I_FRAME_STREAM_INF
{
    public sealed class ExtIFrameStreamInfLexer : Lexer<ExtIFrameStreamInf>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public ExtIFrameStreamInfLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtIFrameStreamInf> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtIFrameStreamInf>.FromResult(new ExtIFrameStreamInf(result.Element));
            }
            return
                ReadResult<ExtIFrameStreamInf>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
