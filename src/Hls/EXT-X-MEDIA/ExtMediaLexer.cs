using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_MEDIA
{
    public sealed class ExtMediaLexer : Lexer<ExtMedia>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public ExtMediaLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtMedia> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtMedia>.FromResult(new ExtMedia(result.Element));
            }
            return ReadResult<ExtMedia>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
