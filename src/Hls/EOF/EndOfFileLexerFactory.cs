using Txt.Core;

namespace Hls.EOF
{
    public class EndOfFileLexerFactory : ILexerFactory<EndOfFile>
    {
        public ILexer<EndOfFile> Create()
        {
            return new EndOfFileLexer();
        }
    }
}
