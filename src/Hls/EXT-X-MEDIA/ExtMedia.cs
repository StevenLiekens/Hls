using Txt.ABNF;

namespace Hls.EXT_X_MEDIA
{
    public class ExtMedia : Concatenation
    {
        public ExtMedia(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
