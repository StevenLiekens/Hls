using System;
using Txt.ABNF;
using Txt.Core;

namespace Hls.enumerated_string
{
    public sealed class EnumeratedStringLexer : Lexer<EnumeratedString>
    {
        private readonly ILexer<Repetition> innerLexer;

        public EnumeratedStringLexer(ILexer<Repetition> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<EnumeratedString> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<EnumeratedString>.FromResult(new EnumeratedString(result.Element));
            }
            return ReadResult<EnumeratedString>.FromSyntaxError(
                SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
