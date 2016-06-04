using System;
using System.Linq;
using Txt.Core;

namespace Hls.hexadecimal_sequence
{
    public class HexadecimalSequenceParser : Parser<HexadecimalSequence, byte[]>
    {
        protected override byte[] ParseImpl(HexadecimalSequence value)
        {
            return Enumerable.Range(0, value.Text.Length)
                        .Where(x => x % 2 == 0)
                        .Select(x => Convert.ToByte(value.Text.Substring(x, 2), 16))
                        .ToArray();
        }
    }
}
