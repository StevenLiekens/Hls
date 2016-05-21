using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.duration
{
    public sealed class DurationLexer : Lexer<Duration>
    {
        private readonly ILexer<Alternation> innerLexer;

        public DurationLexer(ILexer<Alternation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<Duration> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<Duration>.FromResult(new Duration(result.Element));
            }
            return ReadResult<Duration>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
