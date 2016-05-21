using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.tag
{
    public sealed class TagLexer : Lexer<Tag>
    {
        private readonly ILexer<Alternation> innerLexer;

        public TagLexer(ILexer<Alternation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<Tag> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<Tag>.FromResult(new Tag(result.Element));
            }
            return ReadResult<Tag>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
