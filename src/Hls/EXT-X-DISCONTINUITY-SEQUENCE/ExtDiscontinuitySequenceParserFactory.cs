using System;
using Hls.decimal_integer;
using JetBrains.Annotations;
using Txt.Core;

namespace Hls.EXT_X_DISCONTINUITY_SEQUENCE
{
    public class ExtDiscontinuitySequenceParserFactory : ParserFactory<ExtDiscontinuitySequence, int>
    {
        static ExtDiscontinuitySequenceParserFactory()
        {
            Default =
                new ExtDiscontinuitySequenceParserFactory(
                    decimal_integer.DecimalIntegerParserFactory.Default.Singleton());
        }

        public ExtDiscontinuitySequenceParserFactory(
            [NotNull] IParserFactory<DecimalInteger, int> decimalIntegerParserFactory)
        {
            if (decimalIntegerParserFactory == null)
            {
                throw new ArgumentNullException(nameof(decimalIntegerParserFactory));
            }
            DecimalIntegerParserFactory = decimalIntegerParserFactory;
        }

        public static ExtDiscontinuitySequenceParserFactory Default { get; }

        public IParserFactory<DecimalInteger, int> DecimalIntegerParserFactory { get; }

        public override IParser<ExtDiscontinuitySequence, int> Create()
        {
            return new ExtDiscontinuitySequenceParser(DecimalIntegerParserFactory.Create());
        }
    }
}
