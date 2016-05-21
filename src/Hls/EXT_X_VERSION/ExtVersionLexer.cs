using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.EXT_X_VERSION
{
    public sealed class ExtVersionLexer : Lexer<ExtVersion>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public ExtVersionLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtVersion> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtVersion>.FromResult(new ExtVersion(result.Element));
            }
            return ReadResult<ExtVersion>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
