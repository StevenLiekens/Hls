using System;
using Hls.decimal_floating_point;
using JetBrains.Annotations;
using Txt.Core;

namespace Hls.signed_decimal_floating_point
{
    public class SignedDecimalFloatingPointParserFactory : ParserFactory<SignedDecimalFloatingPoint, float>
    {
        static SignedDecimalFloatingPointParserFactory()
        {
            Default = new SignedDecimalFloatingPointParserFactory(
                decimal_floating_point.DecimalFloatingPointParserFactory.Default.Singleton());
        }

        public SignedDecimalFloatingPointParserFactory(
            [NotNull] IParserFactory<DecimalFloatingPoint, float> decimalFloatingPointParserFactory)
        {
            if (decimalFloatingPointParserFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalFloatingPointParserFactory));
            }
            DecimalFloatingPointParserFactory = decimalFloatingPointParserFactory;
        }

        public static IParserFactory<SignedDecimalFloatingPoint, float> Default { get; }

        public IParserFactory<DecimalFloatingPoint, float> DecimalFloatingPointParserFactory { get; }

        public override IParser<SignedDecimalFloatingPoint, float> Create()
        {
            return new SignedDecimalFloatingPointParser(DecimalFloatingPointParserFactory.Create());
        }
    }
}
