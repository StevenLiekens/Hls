using System;
using Hls.decimal_floating_point;
using Hls.decimal_integer;
using Hls.decimal_resolution;
using Hls.hexadecimal_sequence;
using Hls.quoted_string;
using Hls.signed_decimal_floating_point;
using Txt.Core;

namespace Hls.attribute_value
{
    public class AttributeValueParser : Parser<AttributeValue, object>
    {
        private readonly IParser<DecimalFloatingPoint, float> decimalFloatingPointParser;

        private readonly IParser<DecimalInteger, int> decimalIntegerParser;

        private readonly IParser<DecimalResolution, Tuple<int, int>> decimalResolutionParser;

        private readonly IParser<HexadecimalSequence, byte[]> hexadecimalSequenceParser;

        private readonly IParser<QuotedString, string> quotedStringParser;

        private readonly IParser<SignedDecimalFloatingPoint, float> signedDecimalFloatingPointParser;

        public AttributeValueParser(
            IParser<HexadecimalSequence, byte[]> hexadecimalSequenceParser,
            IParser<DecimalResolution, Tuple<int, int>> decimalResolutionParser,
            IParser<DecimalFloatingPoint, float> decimalFloatingPointParser,
            IParser<SignedDecimalFloatingPoint, float> signedDecimalFloatingPointParser,
            IParser<DecimalInteger, int> decimalIntegerParser,
            IParser<QuotedString, string> quotedStringParser)
        {
            this.hexadecimalSequenceParser = hexadecimalSequenceParser;
            this.decimalResolutionParser = decimalResolutionParser;
            this.decimalFloatingPointParser = decimalFloatingPointParser;
            this.signedDecimalFloatingPointParser = signedDecimalFloatingPointParser;
            this.decimalIntegerParser = decimalIntegerParser;
            this.quotedStringParser = quotedStringParser;
        }

        protected override object ParseImpl(AttributeValue value)
        {
            switch (value.Ordinal)
            {
                case 1:
                    return hexadecimalSequenceParser.Parse((HexadecimalSequence)value.Element);
                case 2:
                    return decimalResolutionParser.Parse((DecimalResolution)value.Element);
                case 3:
                    return decimalFloatingPointParser.Parse((DecimalFloatingPoint)value.Element);
                case 4:
                    return signedDecimalFloatingPointParser.Parse((SignedDecimalFloatingPoint)value.Element);
                case 5:
                    return decimalIntegerParser.Parse((DecimalInteger)value.Element);
                case 6:
                    return quotedStringParser.Parse((QuotedString)value.Element);
                case 7:
                    return value.Text;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }
}
