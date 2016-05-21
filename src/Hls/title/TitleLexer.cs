using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.title
{
    public sealed class TitleLexer : Lexer<Title>
    {
        private readonly ILexer<Repetition> innerLexer;

        public TitleLexer(ILexer<Repetition> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<Title> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<Title>.FromResult(new Title(result.Element));
            }
            return ReadResult<Title>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
