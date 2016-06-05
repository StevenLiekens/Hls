using System;
using Hls.decimal_integer;
using Txt.Core;

namespace Hls.EXT_X_DISCONTINUITY_SEQUENCE
{
    public class ExtDiscontinuitySequenceParser : Parser<ExtDiscontinuitySequence, int>
    {
        private readonly IParser<DecimalInteger, int> decimalIntegerParser;

        public ExtDiscontinuitySequenceParser(IParser<DecimalInteger, int> decimalIntegerParser)
        {
            if (decimalIntegerParser == null)
            {
                throw new ArgumentNullException(nameof(decimalIntegerParser));
            }
            this.decimalIntegerParser = decimalIntegerParser;
        }

        protected override int ParseImpl(ExtDiscontinuitySequence value)
        {
            return decimalIntegerParser.Parse((DecimalInteger)value[1]);
        }
    }
}
