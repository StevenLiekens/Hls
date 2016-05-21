using Txt.ABNF;

namespace Hls.duration
{
    public class Duration : Alternation
    {
        public Duration(Alternation Alternation)
            : base(Alternation)
        {
        }
    }
}
