using Txt.Core;

namespace Hls.EXT_X_VERSION
{
    public class ExtVersionParser : Parser<ExtVersion, int>
    {
        protected override int ParseImpl(ExtVersion value)
        {
            return int.Parse(value[1].Text);
        }
    }
}
