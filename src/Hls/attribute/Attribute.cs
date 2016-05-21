using Txt.ABNF;

namespace Hls.attribute
{
    public class Attribute : Concatenation
    {
        public Attribute(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
