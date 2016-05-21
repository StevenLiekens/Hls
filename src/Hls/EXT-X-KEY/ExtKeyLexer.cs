using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_KEY
{
    public sealed class ExtKeyLexer : Lexer<ExtKey>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public ExtKeyLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtKey> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtKey>.FromResult(new ExtKey(result.Element));
            }
            return ReadResult<ExtKey>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
