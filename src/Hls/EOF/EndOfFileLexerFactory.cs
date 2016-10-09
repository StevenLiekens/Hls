using Txt.Core;

namespace Hls.EOF
{
    public class EndOfFileLexerFactory : LexerFactory<EndOfFile>
    {
        public static EndOfFileLexerFactory Default { get; } = new EndOfFileLexerFactory();

        public override ILexer<EndOfFile> Create()
        {
            return new EndOfFileLexer();
        }
    }
}
