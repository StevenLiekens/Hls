using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.EXT_X_ENDLIST
{
    public sealed class ExtEndListLexer : Lexer<ExtEndList>
    {
        private readonly ILexer<Terminal> innerLexer;

        public ExtEndListLexer(ILexer<Terminal> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<ExtEndList> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<ExtEndList>.FromResult(new ExtEndList(result.Element));
            }
            return ReadResult<ExtEndList>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
