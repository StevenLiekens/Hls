using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXT_X_MEDIA_SEQUENCE
{
    public sealed class ExtMediaSequenceLexer : Lexer<ExtMediaSequence>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public ExtMediaSequenceLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtMediaSequence> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtMediaSequence>.FromResult(new ExtMediaSequence(result.Element));
            }
            return ReadResult<ExtMediaSequence>.FromSyntaxError(
                SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
