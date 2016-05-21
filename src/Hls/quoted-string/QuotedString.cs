using Txt.ABNF;

namespace Hls.quoted_string
{
    public class QuotedString : Concatenation
    {
        public QuotedString(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
