using Txt.ABNF;

namespace Hls.tag
{
    public class Tag : Alternation
    {
        public Tag(Alternation Alternation)
            : base(Alternation)
        {
        }
    }
}
