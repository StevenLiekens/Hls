using Txt.Core;

namespace Hls.quoted_string
{
    public class QuotedStringParserFactory : ParserFactory<QuotedString, string>
    {
        static QuotedStringParserFactory()
        {
            Default = new QuotedStringParserFactory();
        }

        public static IParserFactory<QuotedString, string> Default { get; }

        public override IParser<QuotedString, string> Create()
        {
            return new QuotedStringParser();
        }
    }
}
