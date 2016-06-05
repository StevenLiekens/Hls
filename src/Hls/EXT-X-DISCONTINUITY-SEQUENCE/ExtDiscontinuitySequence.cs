using Txt.ABNF;

namespace Hls.EXT_X_DISCONTINUITY_SEQUENCE
{
    public class ExtDiscontinuitySequence : Concatenation
    {
        public ExtDiscontinuitySequence(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
