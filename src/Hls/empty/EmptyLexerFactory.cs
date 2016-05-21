using Txt.Core;

namespace Hls.empty
{
    public class EmptyLexerFactory : ILexerFactory<Empty>
    {
        public ILexer<Empty> Create()
        {
            return new EmptyLexer();
        }
    }
}
