using System;
using Hls.decimal_floating_point;
using Hls.decimal_integer;
using Hls.decimal_resolution;
using Hls.hexadecimal_sequence;
using Hls.quoted_string;
using Hls.signed_decimal_floating_point;
using JetBrains.Annotations;
using Txt.Core;

namespace Hls.attribute_value
{
    public class AttributeValueParserFactory : ParserFactory<AttributeValue, object>
    {
        static AttributeValueParserFactory()
        {
            Default = new AttributeValueParserFactory(
                hexadecimal_sequence.HexadecimalSequenceParserFactory.Default.Singleton(),
                decimal_resolution.DecimalResolutionParserFactory.Default.Singleton(),
                decimal_floating_point.DecimalFloatingPointParserFactory.Default.Singleton(),
                signed_decimal_floating_point.SignedDecimalFloatingPointParserFactory.Default.Singleton(),
                decimal_integer.DecimalIntegerParserFactory.Default.Singleton(),
                quoted_string.QuotedStringParserFactory.Default.Singleton());
        }

        public AttributeValueParserFactory(
            [NotNull] IParserFactory<HexadecimalSequence, byte[]> hexadecimalSequenceParserFactory,
            [NotNull] IParserFactory<DecimalResolution, Tuple<int, int>> decimalResolutionParserFactory,
            [NotNull] IParserFactory<DecimalFloatingPoint, float> decimalFloatingPointParserFactory,
            [NotNull] IParserFactory<SignedDecimalFloatingPoint, float> signedDecimalFloatingPointParserFactory,
            [NotNull] IParserFactory<DecimalInteger, int> decimalIntegerParserFactory,
            [NotNull] IParserFactory<QuotedString, string> quotedStringParserFactory)
        {
            if (hexadecimalSequenceParserFactory == null)
            {
                throw new ArgumentNullException(nameof(hexadecimalSequenceParserFactory));
            }
            if (decimalResolutionParserFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalResolutionParserFactory));
            }
            if (decimalFloatingPointParserFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalFloatingPointParserFactory));
            }
            if (signedDecimalFloatingPointParserFactory == null)
            {
                throw new ArgumentNullException(nameof(signedDecimalFloatingPointParserFactory));
            }
            if (decimalIntegerParserFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalIntegerParserFactory));
            }
            if (quotedStringParserFactory == null)
            {
                throw new ArgumentNullException(nameof(quotedStringParserFactory));
            }
            HexadecimalSequenceParserFactory = hexadecimalSequenceParserFactory;
            DecimalResolutionParserFactory = decimalResolutionParserFactory;
            DecimalFloatingPointParserFactory = decimalFloatingPointParserFactory;
            SignedDecimalFloatingPointParserFactory = signedDecimalFloatingPointParserFactory;
            DecimalIntegerParserFactory = decimalIntegerParserFactory;
            QuotedStringParserFactory = quotedStringParserFactory;
        }

        public static IParserFactory<AttributeValue, object> Default { get; }

        public IParserFactory<DecimalFloatingPoint, float> DecimalFloatingPointParserFactory { get; }

        public IParserFactory<DecimalInteger, int> DecimalIntegerParserFactory { get; }

        public IParserFactory<DecimalResolution, Tuple<int, int>> DecimalResolutionParserFactory { get; }

        public IParserFactory<HexadecimalSequence, byte[]> HexadecimalSequenceParserFactory { get; }

        public IParserFactory<QuotedString, string> QuotedStringParserFactory { get; }

        public IParserFactory<SignedDecimalFloatingPoint, float> SignedDecimalFloatingPointParserFactory { get; }

        public override IParser<AttributeValue, object> Create()
        {
            return new AttributeValueParser(
                HexadecimalSequenceParserFactory.Create(),
                DecimalResolutionParserFactory.Create(),
                DecimalFloatingPointParserFactory.Create(),
                SignedDecimalFloatingPointParserFactory.Create(),
                DecimalIntegerParserFactory.Create(),
                QuotedStringParserFactory.Create());
        }
    }
}
