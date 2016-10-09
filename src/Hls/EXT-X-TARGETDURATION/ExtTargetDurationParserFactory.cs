using System;
using Txt.Core;

namespace Hls.EXT_X_TARGETDURATION
{
    public class ExtTargetDurationParserFactory : ParserFactory<ExtTargetDuration, TimeSpan>
    {
        static ExtTargetDurationParserFactory()
        {
            Default = new ExtTargetDurationParserFactory();
        }

        public static ExtTargetDurationParserFactory Default { get; }

        public override IParser<ExtTargetDuration, TimeSpan> Create()
        {
            return new ExtTargetDurationParser();
        }
    }
}
