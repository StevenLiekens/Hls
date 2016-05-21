using Txt.ABNF;

namespace Hls.line
{
    public class Line : Concatenation
    {
        public Line(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
