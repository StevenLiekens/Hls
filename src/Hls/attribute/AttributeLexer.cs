using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.attribute
{
    public sealed class AttributeLexer : Lexer<Attribute>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public AttributeLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<Attribute> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<Attribute>.FromResult(new Attribute(result.Element));
            }
            return ReadResult<Attribute>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
