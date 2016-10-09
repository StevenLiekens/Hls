using System;
using Hls.decimal_integer;
using JetBrains.Annotations;
using Txt.Core;

namespace Hls.decimal_resolution
{
    public class DecimalResolutionParserFactory : ParserFactory<DecimalResolution, Tuple<int, int>>
    {
        static DecimalResolutionParserFactory()
        {
            Default = new DecimalResolutionParserFactory(
                decimal_integer.DecimalIntegerParserFactory.Default.Singleton());
        }

        public DecimalResolutionParserFactory([NotNull] IParserFactory<DecimalInteger, int> decimalIntegerParserFactory)
        {
            if (decimalIntegerParserFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalIntegerParserFactory));
            }
            DecimalIntegerParserFactory = decimalIntegerParserFactory;
        }

        public static IParserFactory<DecimalResolution, Tuple<int, int>> Default { get; }

        public IParserFactory<DecimalInteger, int> DecimalIntegerParserFactory { get; }

        public override IParser<DecimalResolution, Tuple<int, int>> Create()
        {
            return new DecimalResolutionParser(DecimalIntegerParserFactory.Create());
        }
    }
}
