using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.attribute_list
{
    public sealed class AttributeListLexer : Lexer<AttributeList>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public AttributeListLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<AttributeList> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<AttributeList>.FromResult(new AttributeList(result.Element));
            }
            return ReadResult<AttributeList>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
