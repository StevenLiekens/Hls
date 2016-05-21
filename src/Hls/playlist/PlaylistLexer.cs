using System;
using Txt.Core;
using Txt.ABNF;

namespace Hls.playlist
{
    public sealed class PlaylistLexer : Lexer<Playlist>
    {
        private readonly ILexer<Concatenation> innerLexer;

        public PlaylistLexer(ILexer<Concatenation> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<Playlist> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                if (!scanner.EndOfInput)
                {
                    throw new InvalidOperationException("Expected <EOF>");
                }
                return ReadResult<Playlist>.FromResult(new Playlist(result.Element));
            }
            return ReadResult<Playlist>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
