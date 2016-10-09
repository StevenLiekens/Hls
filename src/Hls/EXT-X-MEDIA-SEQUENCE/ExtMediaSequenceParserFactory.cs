using Txt.Core;

namespace Hls.EXT_X_MEDIA_SEQUENCE
{
    public class ExtMediaSequenceParserFactory : ParserFactory<ExtMediaSequence, int>
    {
        static ExtMediaSequenceParserFactory()
        {
            Default = new ExtMediaSequenceParserFactory();
        }

        public static ExtMediaSequenceParserFactory Default { get; }

        public override IParser<ExtMediaSequence, int> Create()
        {
            return new ExtMediaSequenceParser();
        }
    }
}
