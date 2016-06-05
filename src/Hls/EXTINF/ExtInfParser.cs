using System;
using Hls.duration;
using Txt.Core;

namespace Hls.EXTINF
{
    public class ExtInfParser : Parser<ExtInf, Tuple<TimeSpan, string>>
    {
        private readonly IParser<Duration, TimeSpan> durationParser;

        public ExtInfParser(IParser<Duration, TimeSpan> durationParser)
        {
            if (durationParser == null)
            {
                throw new ArgumentNullException(nameof(durationParser));
            }
            this.durationParser = durationParser;
        }

        protected override Tuple<TimeSpan, string> ParseImpl(ExtInf value)
        {
            var duration = durationParser.Parse((Duration)value[1]);
            string title = null;
            if (value[3].Count == 1)
            {
                title = value[3].Text;
            }
            return new Tuple<TimeSpan, string>(duration, title);
        }
    }
}
