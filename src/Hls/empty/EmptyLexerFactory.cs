using Txt.Core;

namespace Hls.empty
{
    public class EmptyLexerFactory : LexerFactory<Empty>
    {
        public static EmptyLexerFactory Default { get; } = new EmptyLexerFactory();

        public override ILexer<Empty> Create()
        {
            return new EmptyLexer();
        }
    }
}
