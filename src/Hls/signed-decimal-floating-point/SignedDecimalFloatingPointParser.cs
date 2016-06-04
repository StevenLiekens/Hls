using System;
using Hls.decimal_floating_point;
using Txt.Core;

namespace Hls.signed_decimal_floating_point
{
    public class SignedDecimalFloatingPointParser : Parser<SignedDecimalFloatingPoint, float>
    {
        private readonly IParser<DecimalFloatingPoint, float> decimalFloatingPointParser;

        public SignedDecimalFloatingPointParser(IParser<DecimalFloatingPoint, float> decimalFloatingPointParser)
        {
            if (decimalFloatingPointParser == null)
            {
                throw new ArgumentNullException(nameof(decimalFloatingPointParser));
            }
            this.decimalFloatingPointParser = decimalFloatingPointParser;
        }

        protected override float ParseImpl(SignedDecimalFloatingPoint signedDecimalFloatingPoint)
        {
            var sign = signedDecimalFloatingPoint[0].Text;
            var value = decimalFloatingPointParser.Parse((DecimalFloatingPoint)signedDecimalFloatingPoint[1]);
            if (sign == "-")
            {
                return -value;
            }
            return value;
        }
    }
}
