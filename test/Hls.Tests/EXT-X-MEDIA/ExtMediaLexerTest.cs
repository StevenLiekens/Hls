using Txt.Core;
using Xunit;

namespace Hls.EXT_X_MEDIA
{
    public class ExtMediaLexerTest : TestBase
    {
        [Theory]
        [InlineData("#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=\"aac\",NAME=\"English\",DEFAULT=YES,AUTOSELECT=YES,LANGUAGE=\"en\",URI=\"main/english-audio.m3u8\"")]
        public void Read(string value)
        {
            using (var s = new StringTextSource(value))
            using (var sc = new TextScanner(s))
            {
                var lexer = Container.GetInstance<ILexer<ExtMedia>>();
                var result = lexer.Read(sc);
                Assert.Equal(value, result.Text);
            }
        }
    }
}
