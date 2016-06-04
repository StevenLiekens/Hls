using System.Globalization;
using Txt.Core;

namespace Hls.decimal_integer
{
    public class DecimalIntegerParser : Parser<DecimalInteger, int>
    {
        protected override int ParseImpl(DecimalInteger value)
        {
            return int.Parse(value.Text, NumberStyles.None, NumberFormatInfo.InvariantInfo);
        }
    }
}
