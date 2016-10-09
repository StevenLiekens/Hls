using Txt.Core;

namespace Hls.EXT_X_VERSION
{
    public class ExtVersionParserFactory : ParserFactory<ExtVersion, int>
    {
        static ExtVersionParserFactory()
        {
            Default = new ExtVersionParserFactory();
        }

        public static ExtVersionParserFactory Default { get; }

        public override IParser<ExtVersion, int> Create()
        {
            return new ExtVersionParser();
        }
    }
}
