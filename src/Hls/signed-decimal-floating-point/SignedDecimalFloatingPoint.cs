using Txt.ABNF;

namespace Hls.signed_decimal_floating_point
{
    public class SignedDecimalFloatingPoint : Concatenation
    {
        public SignedDecimalFloatingPoint(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
