using Txt.Core;

namespace Hls.decimal_floating_point
{
    public class DecimalFloatingPointParserFactory : ParserFactory<DecimalFloatingPoint, float>
    {
        static DecimalFloatingPointParserFactory()
        {
            Default = new DecimalFloatingPointParserFactory();
        }

        public static IParserFactory<DecimalFloatingPoint, float> Default { get; }

        public override IParser<DecimalFloatingPoint, float> Create()
        {
            return new DecimalFloatingPointParser();
        }
    }
}
