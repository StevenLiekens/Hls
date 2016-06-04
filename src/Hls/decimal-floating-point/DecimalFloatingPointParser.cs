using System.Globalization;
using Txt.Core;

namespace Hls.decimal_floating_point
{
    public class DecimalFloatingPointParser : Parser<DecimalFloatingPoint, float>
    {
        protected override float ParseImpl(DecimalFloatingPoint decimalFloatingPoint)
        {
            return float.Parse(
                decimalFloatingPoint.Text,
                NumberStyles.AllowDecimalPoint,
                NumberFormatInfo.InvariantInfo);
        }
    }
}
