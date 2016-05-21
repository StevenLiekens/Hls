using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.attribute_value
{
    public sealed class AttributeValueLexer : Lexer<AttributeValue>
    {
        private readonly ILexer<Alternation> innerLexer;

        public AttributeValueLexer(ILexer<Alternation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<AttributeValue> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<AttributeValue>.FromResult(new AttributeValue(result.Element));
            }
            return ReadResult<AttributeValue>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
