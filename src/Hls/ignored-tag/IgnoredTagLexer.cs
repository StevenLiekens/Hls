using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.ignored_tag
{
    public sealed class IgnoredTagLexer : Lexer<IgnoredTag>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public IgnoredTagLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<IgnoredTag> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<IgnoredTag>.FromResult(new IgnoredTag(result.Element));
            }
            return ReadResult<IgnoredTag>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
