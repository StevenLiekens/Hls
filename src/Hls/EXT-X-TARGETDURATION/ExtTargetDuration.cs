using Txt.ABNF;

namespace Hls.EXT_X_TARGETDURATION
{
    public class ExtTargetDuration : Concatenation
    {
        public ExtTargetDuration(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
