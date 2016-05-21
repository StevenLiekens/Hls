using Txt.ABNF;

namespace Hls.hexadecimal_sequence
{
    public class HexadecimalSequence : Concatenation
    {
        public HexadecimalSequence(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
