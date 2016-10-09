using System;
using Hls.duration;
using JetBrains.Annotations;
using Txt.Core;

namespace Hls.EXTINF
{
    public class ExtInfParserFactory : ParserFactory<ExtInf, Tuple<TimeSpan, string>>
    {
        static ExtInfParserFactory()
        {
            Default = new ExtInfParserFactory(duration.DurationParserFactory.Default.Singleton());
        }

        public ExtInfParserFactory([NotNull] IParserFactory<Duration, TimeSpan> durationParserFactory)
        {
            if (durationParserFactory == null)
            {
                throw new ArgumentNullException(nameof(durationParserFactory));
            }
            DurationParserFactory = durationParserFactory;
        }

        public static ExtInfParserFactory Default { get; }

        public IParserFactory<Duration, TimeSpan> DurationParserFactory { get; }

        public override IParser<ExtInf, Tuple<TimeSpan, string>> Create()
        {
            return new ExtInfParser(DurationParserFactory.Create());
        }
    }
}
