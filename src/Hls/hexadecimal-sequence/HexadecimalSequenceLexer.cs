using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.hexadecimal_sequence
{
    public sealed class HexadecimalSequenceLexer : Lexer<HexadecimalSequence>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public HexadecimalSequenceLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<HexadecimalSequence> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<HexadecimalSequence>.FromResult(new HexadecimalSequence(result.Element));
            }
            return
                ReadResult<HexadecimalSequence>.FromSyntaxError(
                    SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
