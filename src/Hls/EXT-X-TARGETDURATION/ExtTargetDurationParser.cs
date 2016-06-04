using System;
using Txt.Core;

namespace Hls.EXT_X_TARGETDURATION
{
    public class ExtTargetDurationParser : Parser<ExtTargetDuration, TimeSpan>
    {
        protected override TimeSpan ParseImpl(ExtTargetDuration value)
        {
            return TimeSpan.FromSeconds(int.Parse(value[1].Text));
        }
    }
}
