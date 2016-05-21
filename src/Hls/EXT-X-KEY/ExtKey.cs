using Txt.ABNF;

namespace Hls.EXT_X_KEY
{
    public class ExtKey : Concatenation
    {
        public ExtKey(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
