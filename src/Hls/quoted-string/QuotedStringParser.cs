using Txt.Core;

namespace Hls.quoted_string
{
    public class QuotedStringParser : Parser<QuotedString, string>
    {
        protected override string ParseImpl(QuotedString value)
        {
            return value[1].Text;
        }
    }
}
