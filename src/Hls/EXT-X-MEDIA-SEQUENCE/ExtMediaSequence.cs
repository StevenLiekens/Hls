using Txt.ABNF;

namespace Hls.EXT_X_MEDIA_SEQUENCE
{
    public class ExtMediaSequence : Concatenation
    {
        public ExtMediaSequence(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
