using Txt.Core;

namespace Hls.decimal_integer
{
    public class DecimalIntegerParserFactory : ParserFactory<DecimalInteger, int>
    {
        static DecimalIntegerParserFactory()
        {
            Default = new DecimalIntegerParserFactory();
        }

        public static IParserFactory<DecimalInteger, int> Default { get; }

        public override IParser<DecimalInteger, int> Create()
        {
            return new DecimalIntegerParser();
        }
    }
}
