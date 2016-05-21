using Txt.ABNF;

namespace Hls.EOL
{
    public class EndOfLine : Alternation
    {
        public EndOfLine(Alternation alternation)
            : base(alternation)
        {
        }
    }
}
