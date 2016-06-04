using System;
using Hls.decimal_integer;
using Txt.Core;

namespace Hls.decimal_resolution
{
    public class DecimalResolutionParser : Parser<DecimalResolution, Tuple<int, int>>
    {
        private readonly IParser<DecimalInteger, int> decimalIntegerParser;

        public DecimalResolutionParser(IParser<DecimalInteger, int> decimalIntegerParser)
        {
            if (decimalIntegerParser == null)
            {
                throw new ArgumentNullException(nameof(decimalIntegerParser));
            }
            this.decimalIntegerParser = decimalIntegerParser;
        }

        protected override Tuple<int, int> ParseImpl(DecimalResolution decimalResolution)
        {
            var width = decimalIntegerParser.Parse((DecimalInteger)decimalResolution[0]);
            var height = decimalIntegerParser.Parse((DecimalInteger)decimalResolution[2]);
            return new Tuple<int, int>(width, height);
        }
    }
}
