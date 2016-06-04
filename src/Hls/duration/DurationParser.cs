using System;
using System.Globalization;
using Txt.Core;

namespace Hls.duration
{
    public class DurationParser : Parser<Duration, TimeSpan>
    {
        protected override TimeSpan ParseImpl(Duration duration)
        {
            return TimeSpan.FromSeconds(
                double.Parse(
                    duration.Text,
                    NumberStyles.AllowDecimalPoint,
                    NumberFormatInfo.InvariantInfo));
        }
    }
}
