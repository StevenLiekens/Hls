using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.attribute_name
{
    public sealed class AttributeNameLexer : Lexer<AttributeName>
    {
        private readonly ILexer<Repetition> innerLexer;

        public AttributeNameLexer(ILexer<Repetition> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<AttributeName> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<AttributeName>.FromResult(new AttributeName(result.Element));
            }
            return ReadResult<AttributeName>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
