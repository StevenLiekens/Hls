using System;
using Txt.Core;

namespace Hls.duration
{
    public class DurationParserFactory : ParserFactory<Duration, TimeSpan>
    {
        static DurationParserFactory()
        {
            Default = new DurationParserFactory();
        }

        public static IParserFactory<Duration, TimeSpan> Default { get; }

        public override IParser<Duration, TimeSpan> Create()
        {
            return new DurationParser();
        }
    }
}
