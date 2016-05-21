using Txt.ABNF;

namespace Hls.EXT_X_STREAM_INF
{
    public class ExtStreamInf : Concatenation
    {
        public ExtStreamInf(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
