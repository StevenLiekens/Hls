using Txt.ABNF;

namespace Hls.EXT_X_VERSION
{
    public class ExtVersion : Concatenation
    {
        public ExtVersion(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
