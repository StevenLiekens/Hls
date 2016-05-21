using Txt.ABNF;

namespace Hls.comment
{
    public class Comment : Concatenation
    {
        public Comment(Concatenation concatenation)
            : base(concatenation)
        {
        }
    }
}
