using Txt.ABNF;

namespace Hls.title
{
    public class Title : Repetition
    {
        public Title(Repetition repetition)
            : base(repetition)
        {
        }
    }
}
