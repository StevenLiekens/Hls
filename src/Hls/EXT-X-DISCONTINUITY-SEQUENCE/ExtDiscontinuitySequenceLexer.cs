using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_DISCONTINUITY_SEQUENCE
{
    public sealed class ExtDiscontinuitySequenceLexer : Lexer<ExtDiscontinuitySequence>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public ExtDiscontinuitySequenceLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtDiscontinuitySequence> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtDiscontinuitySequence>.FromResult(new ExtDiscontinuitySequence(result.Element));
            }
            return
                ReadResult<ExtDiscontinuitySequence>.FromSyntaxError(
                    SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
