using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXT_X_TARGETDURATION
{
    public sealed class ExtTargetDurationLexer : Lexer<ExtTargetDuration>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public ExtTargetDurationLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtTargetDuration> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtTargetDuration>.FromResult(new ExtTargetDuration(result.Element));
            }
            return
                ReadResult<ExtTargetDuration>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
