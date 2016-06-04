using Txt.Core;

namespace Hls.EXT_X_MEDIA_SEQUENCE
{
    public class ExtMediaSequenceParser : Parser<ExtMediaSequence, int>
    {
        protected override int ParseImpl(ExtMediaSequence value)
        {
            return int.Parse(value[1].Text);
        }
    }
}
